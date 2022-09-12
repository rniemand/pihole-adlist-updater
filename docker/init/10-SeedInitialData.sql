INSERT INTO `AdLists`
	(`AdListType`, `AdListSource`, `Maintainer`, `ProjectUrl`, `ListUrl`)
VALUES
  -- 9: Suspicious
  (9, 2, 'Filters Heroes'                       , 'https://github.com/FiltersHeroes/KADhosts'                   , 'https://raw.githubusercontent.com/PolishFiltersTeam/KADhosts/master/KADhosts.txt'),
  (9, 2, 'FadeMind'                             , 'https://github.com/FadeMind/hosts.extras'                    , 'https://raw.githubusercontent.com/FadeMind/hosts.extras/master/add.Spam/hosts'),
  (9, 3, 'firebog'                              , 'https://firebog.net/'                                        , 'https://v.firebog.net/hosts/static/w3kbl.txt'),

  -- 2: Advertising
  (2, 4, 'AdAway'                               , 'https://adaway.org/'                                         , 'https://adaway.org/hosts.txt'),
  (2, 3, 'firebog'                              , 'https://firebog.net/'                                        , 'https://v.firebog.net/hosts/AdguardDNS.txt'),
  (2, 3, 'firebog'                              , 'https://firebog.net/'                                        , 'https://v.firebog.net/hosts/Admiral.txt'),
  (2, 2, 'anudeepND'                            , 'https://github.com/anudeepND/blacklist'                      , 'https://raw.githubusercontent.com/anudeepND/blacklist/master/adservers.txt'),
  (2, 5, 'Disconnect.me'                        , 'https://disconnect.me/'                                      , 'https://s3.amazonaws.com/lists.disconnect.me/simple_ad.txt'),
  (2, 3, 'firebog'                              , 'https://firebog.net/'                                        , 'https://v.firebog.net/hosts/Easylist.txt'),
  (2, 2, 'FadeMind'                             , 'https://github.com/FadeMind/hosts.extras'                    , 'https://raw.githubusercontent.com/FadeMind/hosts.extras/master/UncheckyAds/hosts'),
  (2, 6, 'oisd'                                 , 'https://oisd.nl/'                                            , 'https://abp.oisd.nl/basic/'),
  (2, 2, 'DRSDavidSoft'                         , 'https://github.com/DRSDavidSoft/additional-hosts'            , 'https://raw.githubusercontent.com/DRSDavidSoft/additional-hosts/master/domains/blacklist/adservers-and-trackers.txt'),
  (2, 2, 'r-a-y'                                , 'https://github.com/r-a-y/mobile-hosts'                       , 'https://raw.githubusercontent.com/r-a-y/mobile-hosts/master/AdguardMobileAds.txt'),
  (2, 2, 'The Block List Project'               , 'https://github.com/blocklistproject'                         , 'https://blocklistproject.github.io/Lists/alt-version/ads-nl.txt'),
  (2, 3, 'firebog'                              , 'https://firebog.net/'                                        , 'https://v.firebog.net/hosts/Prigent-Ads.txt'),
  (2, 2, 'anudeepND'                            , 'https://github.com/anudeepND/blacklist'                      , 'https://raw.githubusercontent.com/anudeepND/blacklist/master/adservers.txt'),

  -- 3: Tracking
  (3, 3, 'firebog'                              , 'https://firebog.net/'                                        , 'https://v.firebog.net/hosts/Easyprivacy.txt'),
  (3, 2, 'FadeMind'                             , 'https://github.com/FadeMind/hosts.extras'                    , 'https://raw.githubusercontent.com/FadeMind/hosts.extras/master/add.2o7Net/hosts'),
  (3, 2, 'crazy-max'                            , 'https://github.com/crazy-max/WindowsSpyBlocker'              , 'https://raw.githubusercontent.com/crazy-max/WindowsSpyBlocker/master/data/hosts/spy.txt'),
  (3, 6, 'Geoffrey Frogeye'                     , 'https://hostfiles.frogeye.fr/'                               , 'https://hostfiles.frogeye.fr/firstparty-trackers-hosts.txt'),
  (3, 2, 'lightswitch05'                        , 'https://github.com/lightswitch05/hosts'                      , 'https://raw.githubusercontent.com/lightswitch05/hosts/master/docs/lists/tracking-aggressive-extended.txt'),
  (3, 5, 'Disconnect.me'                        , 'https://disconnect.me/'                                      , 'https://s3.amazonaws.com/lists.disconnect.me/simple_tracking.txt'),
  (3, 7, 'Spirillen Marsupilami'                , 'https://gitlab.com/my-privacy-dns/matrix/matrix'             , 'https://gitlab.com/my-privacy-dns/matrix/matrix/-/raw/master/source/tracking/domains.list'),
  (3, 2, 'RooneyMcNibNug'                       , 'https://github.com/RooneyMcNibNug/pihole-stuff'              , 'https://raw.githubusercontent.com/RooneyMcNibNug/pihole-stuff/master/SNAFU.txt'),

  -- 6: Other
  (6, 6, 'Peter Lowe'                           , 'https://patreon.com/blocklist'                               , 'https://pgl.yoyo.org/adservers/serverlist.php?hostformat=hosts&showintro=0&mimetype=plaintext'),
  (6, 7, 'ZeroDot1'                             , 'https://gitlab.com/ZeroDot1/CoinBlockerLists/-/tree/master/' , 'https://zerodot1.gitlab.io/CoinBlockerLists/hosts_browser'),

  -- 4: Malicious
  (4, 2, 'DandelionSprout'                      , 'https://github.com/DandelionSprout/adfilt'                   , 'https://raw.githubusercontent.com/DandelionSprout/adfilt/master/Alternate%20versions%20Anti-Malware%20List/AntiMalwareHosts.txt'),
  (4, 6, 'DigitalSide Threat-Intel Repository'  , 'https://osint.digitalside.it/'                               , 'https://osint.digitalside.it/Threat-Intel/lists/latestdomains.txt'),
  (4, 5, 'Disconnect.me'                        , 'https://disconnect.me/'                                      , 'https://s3.amazonaws.com/lists.disconnect.me/simple_malvertising.txt'),
  (4, 3, 'firebog'                              , 'https://firebog.net/'                                        , 'https://v.firebog.net/hosts/Prigent-Crypto.txt'),
  (4, 2, 'FadeMind'                             , 'https://github.com/FadeMind/hosts.extras'                    , 'https://raw.githubusercontent.com/FadeMind/hosts.extras/master/add.Risk/hosts'),
  (4, 8, 'Ethan Robish'                         , 'https://bitbucket.org/ethanr/dns-blacklists/src/master/'     , 'https://bitbucket.org/ethanr/dns-blacklists/raw/8575c9f96e5b4a1308f2f12394abd86d0927a4a0/bad_lists/Mandiant_APT1_Report_Appendix_D.txt'),
  (4, 6, 'Phishing Army'                        , 'https://phishing.army/'                                      , 'https://phishing.army/download/phishing_army_blocklist_extended.txt'),
  (4, 7, 'Ming Di Leom'                         , 'https://gitlab.com/malware-filter/urlhaus-filter'            , 'https://malware-filter.gitlab.io/malware-filter/phishing-filter-hosts.txt'),
  (4, 7, 'Quids'                                , 'https://gitlab.com/quidsup/notrack-blocklists'               , 'https://gitlab.com/quidsup/notrack-blocklists/raw/master/notrack-malware.txt'),
  (4, 2, 'Spam404'                              , 'https://github.com/Spam404/lists'                            , 'https://raw.githubusercontent.com/Spam404/lists/master/main-blacklist.txt'),
  (4, 2, 'AssoEchap'                            , 'https://github.com/AssoEchap/stalkerware-indicators'         , 'https://raw.githubusercontent.com/AssoEchap/stalkerware-indicators/master/generated/hosts'),
  (4, 6, 'abuse.ch'                             , 'https://urlhaus.abuse.ch/'                                   , 'https://urlhaus.abuse.ch/downloads/hostfile/'),

  -- 5: Adult
  (5, 3, 'firebog'                              , 'https://firebog.net/'                                        , 'https://v.firebog.net/hosts/Prigent-Adult.txt'),

  -- 7: Spam
  (7, 2, 'Matomo Analytics'                     , 'https://github.com/matomo-org/referrer-spam-blacklist'       , 'https://raw.githubusercontent.com/matomo-org/referrer-spam-blacklist/master/spammers.txt')
;


INSERT INTO `Whitelists`
  (`IsRegex`, `Order`, `Expression`)
VALUES
  (0, 255, 'fonts.gstatic.com'),
  (0, 255, 'cdn.jsdelivr.net'),

  (1, 255, '.*\.whatsapp\.(net|com)$'),
  (1, 255, '.*?\.gvt(\d{1,})\.com');
