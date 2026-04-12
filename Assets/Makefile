.PHONY: png ico clean all

png_dir = png
ico_dir = .
icon_file = icon.svg
icon_file_sm = icon-simple.svg
app_name = galaxy-launcher

all: png ico 

#  Needs rsvg-convert from the librsvg package.
png:
	mkdir -p $(png_dir)

	rsvg-convert -h 16 $(icon_file_sm) > $(png_dir)/icon-16.png
	rsvg-convert -h 20 $(icon_file_sm) > $(png_dir)/icon-20.png
	rsvg-convert -h 22 $(icon_file_sm) > $(png_dir)/icon-22.png
	rsvg-convert -h 24 $(icon_file) > $(png_dir)/icon-24.png
	rsvg-convert -h 30 $(icon_file) > $(png_dir)/icon-30.png
	rsvg-convert -h 32 $(icon_file) > $(png_dir)/icon-32.png
	rsvg-convert -h 36 $(icon_file) > $(png_dir)/icon-36.png
	rsvg-convert -h 40 $(icon_file) > $(png_dir)/icon-40.png
	rsvg-convert -h 48 $(icon_file) > $(png_dir)/icon-48.png
	rsvg-convert -h 60 $(icon_file) > $(png_dir)/icon-60.png
	rsvg-convert -h 64 $(icon_file) > $(png_dir)/icon-64.png
	rsvg-convert -h 72 $(icon_file) > $(png_dir)/icon-72.png
	rsvg-convert -h 80 $(icon_file) > $(png_dir)/icon-80.png
	rsvg-convert -h 96 $(icon_file) > $(png_dir)/icon-96.png
	rsvg-convert -h 128 $(icon_file) > $(png_dir)/icon-128.png
	rsvg-convert -h 256 $(icon_file) > $(png_dir)/icon-256.png
	rsvg-convert -h 512 $(icon_file) > $(png_dir)/icon-512.png

#  Needs icotool from the icoutils packags (there's no good Windows native
#  CLI tool for this).
ico: png
	mkdir -p $(ico_dir)
	
	icotool -c $(png_dir)/icon-256.png \
	           $(png_dir)/icon-96.png \
	           $(png_dir)/icon-80.png \
	           $(png_dir)/icon-72.png \
	           $(png_dir)/icon-64.png \
	           $(png_dir)/icon-60.png \
	           $(png_dir)/icon-48.png \
	           $(png_dir)/icon-40.png \
	           $(png_dir)/icon-36.png \
	           $(png_dir)/icon-32.png \
	           $(png_dir)/icon-30.png \
	           $(png_dir)/icon-24.png \
	           $(png_dir)/icon-20.png \
	           $(png_dir)/icon-16.png \
	        -o $(ico_dir)/icon.ico

clean:
	rm -rf "$(png_dir)"

