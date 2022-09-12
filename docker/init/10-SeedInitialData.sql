INSERT INTO `AdLists`
	(`AdListType`, `AdListSource`, `Maintainer`, `ProjectUrl`, `ListUrl`)
VALUES
    (9, 2, 'Filters Heroes' , 'https://github.com/FiltersHeroes/KADhosts'   , 'https://raw.githubusercontent.com/PolishFiltersTeam/KADhosts/master/KADhosts.txt'),
    (9, 2, 'FadeMind'       , 'https://github.com/FadeMind/hosts.extras'    , 'https://raw.githubusercontent.com/FadeMind/hosts.extras/master/add.Spam/hosts'),
    (9, 3, 'firebog'        , 'https://firebog.net/'                        , 'https://v.firebog.net/hosts/static/w3kbl.txt');

INSERT INTO `Whitelists`
  (`IsRegex`, `Order`, `Expression`)
VALUES
  (0, 255, 'fonts.gstatic.com'),
  (0, 255, 'cdn.jsdelivr.net'),

  (1, 255, '.*\.whatsapp\.(net|com)$'),
  (1, 255, '.*?\.gvt(\d{1,})\.com');
