import os
import math
from PIL import Image, ImageDraw, ImageFont
from datetime import datetime
from LargeImages import LayoutLevel, Contig, palette, pretty_contig_name, multi_line_height, copytree


class DDVTileLayout:
    def __init__(self):
        # use_fat_headers: For large chromosomes in multipart files, do you change the layout to allow for titles that
        # are outside of the nucleotide coordinate grid?
        self.use_fat_headers = False  # Can only be changed in code.
        self.image = None
        self.draw = None
        self.pixels = None
        self.contigs = []
        self.image_length = 0
        # noinspection PyListCreation
        self.levels = [
            LayoutLevel("XInColumn", 100, 1),  # [0]
            LayoutLevel("LineInColumn", 1000, 100)  # [1]
        ]
        self.levels.append(LayoutLevel("ColumnInRow", 100, levels=self.levels))  # [2]
        self.levels.append(LayoutLevel("RowInTile", 10, levels=self.levels))  # [3]
        self.levels.append(LayoutLevel("XInTile", 3, levels=self.levels))  # [4]
        self.levels.append(LayoutLevel("YInTile", 4, levels=self.levels))  # [5]
        if self.use_fat_headers:
            self.levels[5].padding += self.levels[3].thickness  # one full row for a chromosome title
            self.levels[5].thickness += self.levels[3].thickness
        self.levels.append(LayoutLevel("TileColumn", 9, levels=self.levels))  # [6]
        self.levels.append(LayoutLevel("TileRow", 999, levels=self.levels))  # [7]

        self.tile_label_size = self.levels[3].chunk_size * 2
        if self.use_fat_headers:
            self.tile_label_size = 0  # Fat_headers are not part of the coordinate space

    def process_file(self, input_file_name, output_folder, output_file_name):
        start_time = datetime.now()
        self.image_length = self.read_contigs(input_file_name)
        print("Read contigs :", datetime.now() - start_time)
        self.prepare_image(self.image_length)
        print("Initialized Image:", datetime.now() - start_time, "\n")
        try:  # These try catch statements ensure we get at least some output.  These jobs can take hours
            self.draw_nucleotides()
            print("\nDrew Nucleotides:", datetime.now() - start_time)
        except Exception as e:
            print('Encountered exception while drawing nucleotides:', '\n', str(e))
        try:
            if len(self.contigs) > 1:
                self.draw_titles()
                print("Drew Titles:", datetime.now() - start_time)
        except Exception as e:
            print('Encountered exception while drawing titles:', '\n', str(e))
        try:
            self.generate_html(input_file_name, output_folder)
        except Exception as e:
            print('While generating HTML:', '\n', str(e))
        self.output_image(output_folder, output_file_name)
        print("Output Image in:", datetime.now() - start_time)

    def draw_nucleotides(self):
        total_progress = 0
        # Layout contigs one at a time
        for contig in self.contigs:
            total_progress += contig.reset_padding + contig.title_padding
            seq_length = len(contig.seq)
            for cx in range(0, seq_length, 100):
                x, y = self.position_on_screen(total_progress)
                remaining = min(100, seq_length - cx)
                total_progress += remaining
                for i in range(remaining):
                    self.draw_pixel(contig.seq[cx + i], x + i, y)
            if self.image_length > 10000000:
                print('\r', str(total_progress / self.image_length * 100)[:6], '% done:', contig.name,
                      end="")  # pseudo progress bar
            total_progress += contig.tail_padding  # add trailing white space after the contig sequence body
        print()

    def read_contigs(self, input_file_name):
        self.contigs = []
        total_progress = 0
        current_name = ""
        seq_collection = []

        # Pre-read generates an array of contigs with labels and sequences
        with open(input_file_name, 'r') as streamFASTAFile:
            for read in streamFASTAFile.read().splitlines():
                if read == "":
                    continue
                if read[0] == ">":
                    # If we have sequence gathered and we run into a second (or more) block
                    if len(seq_collection) > 0:
                        sequence = "".join(seq_collection)
                        seq_collection = []  # clear
                        reset, title, tail = self.calc_padding(total_progress, len(sequence), True)
                        self.contigs.append(Contig(current_name, sequence, reset, title, tail))
                        total_progress += reset + title + tail + len(sequence)
                    current_name = read[1:]  # remove >
                else:
                    # collects the sequence to be stored in the contig, constant time performance don't concat strings!
                    seq_collection.append(read)

        # add the last contig to the list
        sequence = "".join(seq_collection)
        reset, title, tail = self.calc_padding(total_progress, len(sequence), len(self.contigs) > 0)
        self.contigs.append(Contig(current_name, sequence, reset, title, tail))
        return total_progress + reset + title + tail + len(sequence)

    def prepare_image(self, image_length):
        width, height = self.max_dimensions(image_length)
        self.image = Image.new('RGB', (width, height), "white")
        self.draw = ImageDraw.Draw(self.image)
        self.pixels = self.image.load()

    def calc_padding(self, total_progress, next_segment_length, multipart_file):
        min_gap = (20 + 6) * 100  # 20px font height, + 6px vertical padding  * 100 nt per line
        if not multipart_file:
            return 0, 0, 0

        for i, current_level in enumerate(self.levels):
            if next_segment_length + min_gap < current_level.chunk_size:
                # give a full level of blank space just in case the previous
                title_padding = max(min_gap, self.levels[i - 1].chunk_size)
                if title_padding > self.levels[3].chunk_size:  # Special case for full tile, don't need to go that big
                    title_padding = self.tile_label_size
                if next_segment_length + title_padding > current_level.chunk_size:
                    continue  # adding the title pushed the above comparison over the edge, step up one level
                space_remaining = current_level.chunk_size - total_progress % current_level.chunk_size
                # sequence comes right up to the edge.  There should always be >= 1 full gap
                reset_level = current_level  # bigger reset when close to filling chunk_size
                if next_segment_length + title_padding < space_remaining:
                    reset_level = self.levels[i - 1]
                    # reset_level = self.levels[i - 1]
                reset_padding = 0
                if total_progress != 0:  # fill out the remainder so we can start at the beginning
                    reset_padding = reset_level.chunk_size - total_progress % reset_level.chunk_size
                total_padding = total_progress + title_padding + reset_padding + next_segment_length
                tail = self.levels[i - 1].chunk_size - total_padding % self.levels[i - 1].chunk_size - 1

                return reset_padding, title_padding, tail

        return 0, 0, 0

    def position_on_screen(self, index):
        """ Readable unoptimized version:
        Maps a nucleotide index to an x,y coordinate based on the rules set in self.levels"""
        xy = [6, 6]  # column padding for various markup = self.levels[2].padding
        if self.use_fat_headers:
            xy[1] += self.levels[5].padding  # padding comes before, not after
        for i, level in enumerate(self.levels):
            if index < level.chunk_size:
                return xy
            part = i % 2
            coordinate_in_chunk = int(index / level.chunk_size) % level.modulo
            xy[part] += level.thickness * coordinate_in_chunk
        return xy

    @staticmethod
    def position_on_screen_small(index):
        # Less readable
        # x = self.levels[0].thickness * (int(index / self.levels[0].chunk_size) % self.levels[0].modulo)
        # x+= self.levels[2].thickness * (int(index / self.levels[2].chunk_size) % self.levels[2].modulo)
        # y = self.levels[1].thickness * (int(index / self.levels[1].chunk_size) % self.levels[1].modulo)
        # y+= self.levels[3].thickness * (int(index / self.levels[3].chunk_size) % self.levels[3].modulo)

        x = index % 100 + 106 * ((index // 100000) % 100) + 10654 * (index // 100000000)  # % 3)
        y = (index // 100) % 1000 + 1018 * ((index // 10000000) % 10)  # + 10342 * ((index // 300000000) % 4)
        return x, y

    @staticmethod
    def position_on_screen_big(index):
        # 10654 * 3 + 486 padding = 32448
        x = index % 100 + 106 * ((index // 100000) % 100) + 10654 * ((index // 100000000) % 3) + \
            32448 * (index // 1200000000)  # % 9 #this will continue tile columns indefinitely (8 needed 4 human genome)
        y = (index // 100) % 1000 + 1018 * ((index // 10000000) % 10) + 10342 * ((index // 300000000) % 4)
        # + 42826 * (index // 10800000000)  # 10342 * 4 + 1458 padding = 42826
        return [x, y]

    def draw_pixel(self, character, x, y):
        self.pixels[x, y] = palette[character]

    def draw_titles(self):
        total_progress = 0
        for contig in self.contigs:
            total_progress += contig.reset_padding  # is to move the cursor to the right line for a large title
            self.draw_title(total_progress, contig)
            total_progress += contig.title_padding + len(contig.seq) + contig.tail_padding

    def draw_title(self, total_progress, contig):
        upper_left = self.position_on_screen(total_progress)
        bottom_right = self.position_on_screen(total_progress + contig.title_padding - 2)
        width, height = bottom_right[0] - upper_left[0], bottom_right[1] - upper_left[1]

        font_size = 9
        title_width = 18
        title_lines = 2

        # Title orientation and size
        if contig.title_padding == self.levels[2].chunk_size:
            # column titles are vertically oriented
            width, height = height, width  # swap
            font_size = 38
            title_width = 50  # TODO: find width programatically
        if contig.title_padding >= self.levels[3].chunk_size:
            font_size = 380  # full row labels for chromosomes
            title_width = 50  # approximate width
        if contig.title_padding == self.tile_label_size:  # Tile dedicated to a Title (square shaped)
            # since this level is square, there's no point in rotating it
            font_size = 380 * 2  # doesn't really need to be 10x larger than the rows
            title_width = 50 // 2
            if self.use_fat_headers:
                # TODO add reset_padding from next contig, just in case there's unused space on this level
                tiles_spanned = math.ceil((len(contig.seq) + contig.tail_padding) / self.levels[4].chunk_size)
                title_width *= tiles_spanned  # Twice the size, but you have 3 tile columns to fill, also limited by 'width'
                title_lines = 1
                upper_left[1] -= self.levels[3].thickness  # above the start of the coordinate grid
                height = self.levels[3].thickness
                width = self.levels[4].thickness * tiles_spanned  # spans 3 full Tiles, or one full Page width

        font = ImageFont.truetype("tahoma.ttf", font_size)
        txt = Image.new('RGBA', (width, height))
        multi_line_title = pretty_contig_name(contig, title_width, title_lines)

        bottom_justified = height - multi_line_height(font, multi_line_title, txt)
        ImageDraw.Draw(txt).multiline_text((0, max(0, bottom_justified)), multi_line_title, font=font,
                                           fill=(0, 0, 0, 255))
        if contig.title_padding == self.levels[2].chunk_size:
            txt = txt.rotate(90, expand=True)
            upper_left[0] += 8  # adjusts baseline for more polish

        self.image.paste(txt, (upper_left[0], upper_left[1]), txt)

    def output_image(self, output_folder, output_file_name):
        del self.pixels
        del self.draw
        self.image.save(os.path.join(output_folder, output_file_name), 'PNG')
        del self.image

    def max_dimensions(self, image_length):
        """ Uses Tile Layout to find the largest chunk size in each dimension (XY) that the
        image_length will reach
        :param image_length: includes sequence length and padding from self.read_contigs()
        :return: width and height needed
        """
        width_height = [0, 0]
        for i, level in enumerate(self.levels):
            part = i % 2
            # how many of these will you need up to a full modulo worth
            coordinate_in_chunk = min(int(math.ceil(image_length / float(level.chunk_size))), level.modulo)
            if coordinate_in_chunk > 1:
                # not cumulative, just take the max size for either x or y
                width_height[part] = max(width_height[part], level.thickness * coordinate_in_chunk)
        if self.use_fat_headers:  # extra margin at the top of the image for a title
            width_height[1] += self.levels[5].padding
        width_height[0] += self.levels[2].padding * 2  # add column padding to both sides
        width_height[1] += self.levels[2].padding * 2  # column padding used as a proxy for vertical padding
        return width_height

    def generate_html(self, input_file_name, output_folder):
        copytree(os.path.join(os.getcwd(), 'html template'), output_folder)  # copies the whole template directory
        html_path = os.path.join(output_folder, 'index.html')
        html_content = {"title": input_file_name[:input_file_name.rfind('.')],
                        "originalImageWidth": str(self.image.width),
                        "originalImageHeight": str(self.image.height),
                        "ColumnPadding": str(self.levels[2].padding),
                        "columnWidthInNucleotides": str(self.levels[1].chunk_size),
                        "layoutSelector": '1',
                        "layout_levels": self.levels_json(),
                        "ContigSpacingJSON": self.contig_json(),
                        "multipart_file": str(len(self.contigs) > 1).lower(),
                        "use_fat_headers": str(self.use_fat_headers).lower(),
                        "includeDensity": 'false',
                        "usa": 'refseq_fetch:' + input_file_name,
                        "ipTotal": str(self.image_length),
                        "direct_data_file": 'sequence.fasta',
                        "direct_data_file_length": str(self.image_length),  # TODO: this isn't right because includes padding
                        "sbegin": '1',
                        "send": str(self.image_length),
                        "output_dir": './',
                        "date": datetime.now().strftime("%Y-%m-%d")}
        with open(os.path.join('html template', 'index.html'), 'r') as template:
            template_content = template.read()
            for key, value in html_content.items():
                template_content = template_content.replace('{{' + key + '}}', value)
            with open(html_path, 'w') as out:
                out.write(template_content)


    def contig_json(self):
        json = []
        starting_index = 0  # camel case is left over from C# for javascript compatibility
        for contig in self.contigs:
            starting_index += contig.title_padding
            end_index = starting_index + len(contig.seq)
            json.append({"name": contig.name.replace("'", ""), "startingIndex": starting_index, "endIndex": end_index,
                         "title_padding": contig.title_padding, "tail_padding": contig.tail_padding})
        return str(json)

    def levels_json(self):
        json = []
        for level in self.levels:
            json.append({"modulo": level.modulo, "chunk_size": level.chunk_size,
                         "padding": level.padding, "thickness": level.thickness})
        return str(json)

    def get_packed_coordinates(self):
        """An attempted speed up for draw_nucleotides() that was the same speed.  In draw_nucleotides() the
        extra code was:
            coordinates = self.get_packed_coordinates()  # precomputed sets of 100,000
            seq_consumed = 0
            columns_batched = 0
            for column in range(0, seq_length, 100000):
                if seq_length - column > 100000:
                    columns_batched += 1
                    x, y = self.position_on_screen(total_progress)  # only one call per column
                    for cx, cy, offset in coordinates:
                        self.draw_pixel(contig.seq[column + offset], x + cx, y + cy)
                    total_progress += 100000
                    seq_consumed += 100000
                else:
                    pass  # loop will exit and remaining seq will be handled individually

        This method is an optimization that computes all offsets for a column once so they can be reused.
        The output looks like this:  (x, y, sequence offset)
        [(0, 0, 0), (1, 0, 1), (2, 0, 2), (3, 0, 3), ... (0, 1, 10), (1, 1, 11), (2, 1, 12), (3, 1, 13),"""
        line = range(self.levels[0].modulo)
        column_height = self.levels[1].modulo
        coords = []
        for y in range(column_height):
            coords.extend([(x, y, y * self.levels[0].modulo + x) for x in line])
        return coords
