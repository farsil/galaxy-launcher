.PHONY: help all install uninstall clean install-png install-exec install-desktop install-data uninstall-png uninstall-exec uninstall-desktop uninstall-data

prefix = /usr/local
exec_prefix = $(prefix)

bindir = $(exec_prefix)/bin
datarootdir = $(prefix)/share
datadir = $(datarootdir)
appdir = $(datarootdir)/applications
icondir = $(datarootdir)/icons

assets_dir = assets
app_name = galaxy-launcher

help:
	@echo "TODO: write help text"

install-png: 
	mkdir -m 0755 -p "$(DESTDIR)$(icondir)/hicolor/16x16/apps/"
	mkdir -m 0755 -p "$(DESTDIR)$(icondir)/hicolor/22x22/apps/"
	mkdir -m 0755 -p "$(DESTDIR)$(icondir)/hicolor/24x24/apps/"
	mkdir -m 0755 -p "$(DESTDIR)$(icondir)/hicolor/32x32/apps/"
	mkdir -m 0755 -p "$(DESTDIR)$(icondir)/hicolor/48x48/apps/"
	mkdir -m 0755 -p "$(DESTDIR)$(icondir)/hicolor/64x64/apps/"
	mkdir -m 0755 -p "$(DESTDIR)$(icondir)/hicolor/96x96/apps/"
	mkdir -m 0755 -p "$(DESTDIR)$(icondir)/hicolor/128x128/apps/"
	mkdir -m 0755 -p "$(DESTDIR)$(icondir)/hicolor/256x256/apps/"
	mkdir -m 0755 -p "$(DESTDIR)$(icondir)/hicolor/scalable/apps/"

	install -p -m 0644 "$(assets_dir)/png/icon-16.png" "$(DESTDIR)$(icondir)/hicolor/16x16/apps/$(app_name).png"
	install -p -m 0644 "$(assets_dir)/png/icon-22.png" "$(DESTDIR)$(icondir)/hicolor/22x22/apps/$(app_name).png"
	install -p -m 0644 "$(assets_dir)/png/icon-24.png" "$(DESTDIR)$(icondir)/hicolor/24x24/apps/$(app_name).png"
	install -p -m 0644 "$(assets_dir)/png/icon-32.png" "$(DESTDIR)$(icondir)/hicolor/32x32/apps/$(app_name).png"
	install -p -m 0644 "$(assets_dir)/png/icon-48.png" "$(DESTDIR)$(icondir)/hicolor/48x48/apps/$(app_name).png"
	install -p -m 0644 "$(assets_dir)/png/icon-64.png" "$(DESTDIR)$(icondir)/hicolor/64x64/apps/$(app_name).png"
	install -p -m 0644 "$(assets_dir)/png/icon-96.png" "$(DESTDIR)$(icondir)/hicolor/96x96/apps/$(app_name).png"
	install -p -m 0644 "$(assets_dir)/png/icon-128.png" "$(DESTDIR)$(icondir)/hicolor/128x128/apps/$(app_name).png"
	install -p -m 0644 "$(assets_dir)/png/icon-256.png" "$(DESTDIR)$(icondir)/hicolor/256x256/apps/$(app_name).png"
	install -p -m 0644 "$(assets_dir)/icon.svg" "$(DESTDIR)$(icondir)/hicolor/scalable/apps/$(app_name).svg"
	
	touch "$(DESTDIR)$(icondir)/hicolor"
	
install-exec:
	mkdir -m 0755 -p "$(DESTDIR)$(bindir)"
	install -p -m 0755 "$(app_name)" "$(DESTDIR)$(bindir)/$(app_name)"

install-desktop: 
	mkdir -m 0755 -p "$(DESTDIR)$(appdir)"
	install -p -m 0644 "$(app_name).desktop" "$(DESTDIR)$(appdir)/$(app_name).desktop"

install-data:
	mkdir -m 0755 -p "$(DESTDIR)$(datadir)"
	mkdir -m 0700 -p "$(DESTDIR)$(datadir)/$(app_name)"
	install -p -m 0644 "USAGE.md" "$(DESTDIR)$(datadir)/$(app_name)/USAGE.md"
	
install: install-png install-exec install-desktop install-data
	        
uninstall-png:
	rm -f "$(DESTDIR)$(icondir)/hicolor/16x16/apps/$(app_name).png"
	rm -f "$(DESTDIR)$(icondir)/hicolor/22x22/apps/$(app_name).png"
	rm -f "$(DESTDIR)$(icondir)/hicolor/24x24/apps/$(app_name).png"
	rm -f "$(DESTDIR)$(icondir)/hicolor/32x32/apps/$(app_name).png"
	rm -f "$(DESTDIR)$(icondir)/hicolor/48x48/apps/$(app_name).png"
	rm -f "$(DESTDIR)$(icondir)/hicolor/64x64/apps/$(app_name).png"
	rm -f "$(DESTDIR)$(icondir)/hicolor/96x96/apps/$(app_name).png"
	rm -f "$(DESTDIR)$(icondir)/hicolor/128x128/apps/$(app_name).png"
	rm -f "$(DESTDIR)$(icondir)/hicolor/256x256/apps/$(app_name).png"
	rm -f "$(DESTDIR)$(icondir)/hicolor/scalable/apps/$(app_name).svg"

uninstall-exec:
	rm -f "$(DESTDIR)$(bindir)/$(app_name)"

uninstall-desktop: 
	rm -f "$(DESTDIR)$(appdir)/$(app_name).desktop"
	
uninstall-data:
	rm -f "$(DESTDIR)$(datadir)/$(app_name)/USAGE.md"

uninstall: uninstall-png uninstall-exec uninstall-desktop uninstall-data
