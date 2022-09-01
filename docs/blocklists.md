# Block List Providers

Most lists used in this project come from: [https://firebog.net/](https://firebog.net/) unless otherwise stated.

For ease of use you can make use of these combined lists, or use one (all) of the more specific lists listed below:

- [combined.txt](https://raw.githubusercontent.com/rniemand/pihole-adlist/master/lists/_combined.txt) - contains entries from the following lists
  - Suspicious
  - Advertising
  - Tracking
  - Malicious
  - Other
- [combined-strict.txt](https://raw.githubusercontent.com/rniemand/pihole-adlist/master/lists/_combined-strict.txt) - contains entries from the following lists
  - Suspicious - `strict`
  - Advertising - `strict`
  - Tracking - `strict`
  - Malicious - `strict`
  - Other - `strict`

You will need to reference both combined list sets if you wish to have everything blocked 😊.

## Suspicious

The [suspicious](https://raw.githubusercontent.com/rniemand/pihole-adlist/master/lists/suspicious.txt) list is comprised of the following sources:

- ([Repo](https://github.com/FiltersHeroes/KADhosts)) :: https://raw.githubusercontent.com/PolishFiltersTeam/KADhosts/master/KADhosts.txt
- ([Repo](https://github.com/FadeMind/hosts.extras)) :: https://raw.githubusercontent.com/FadeMind/hosts.extras/master/add.Spam/hosts
- https://v.firebog.net/hosts/static/w3kbl.txt

The [suspicious-strict](https://raw.githubusercontent.com/rniemand/pihole-adlist/master/lists/suspicious-strict.txt) list is comprised of the following sources:

- ([Repo](https://github.com/matomo-org/referrer-spam-list)) :: https://raw.githubusercontent.com/matomo-org/referrer-spam-blacklist/master/spammers.txt
- ([Site](https://someonewhocares.org/)) :: https://someonewhocares.org/hosts/zero/hosts

## Advertising

The [advertising](https://raw.githubusercontent.com/rniemand/pihole-adlist/master/lists/advertising.txt) list is comprised of the following sources:

- ([Site](https://adaway.org/)) :: https://adaway.org/hosts.txt
- https://v.firebog.net/hosts/AdguardDNS.txt
- https://v.firebog.net/hosts/Admiral.txt
- ([Repo](https://github.com/anudeepND/blacklist)) :: https://raw.githubusercontent.com/anudeepND/blacklist/master/adservers.txt
- https://s3.amazonaws.com/lists.disconnect.me/simple_ad.txt
- https://v.firebog.net/hosts/Easylist.txt
- ([Site](https://pgl.yoyo.org/)) :: https://pgl.yoyo.org/adservers/serverlist.php?hostformat=hosts&showintro=0&mimetype=plaintext
- ([Repo](https://github.com/FadeMind/hosts.extras)) :: https://raw.githubusercontent.com/FadeMind/hosts.extras/master/UncheckyAds/hosts
- ([Repo](https://github.com/bigdargon/hostsVN)) :: https://raw.githubusercontent.com/bigdargon/hostsVN/master/hosts

The [advertising-strict](https://raw.githubusercontent.com/rniemand/pihole-adlist/master/lists/advertising-strict.txt) list is comprised of the following sources:

- ([Repo](https://github.com/jdlingyu/ad-wars)) :: https://raw.githubusercontent.com/jdlingyu/ad-wars/master/hosts

## Tracking

The [tracking](https://raw.githubusercontent.com/rniemand/pihole-adlist/master/lists/tracking.txt) list is comprised of the following sources:

- https://v.firebog.net/hosts/Easyprivacy.txt
- https://v.firebog.net/hosts/Prigent-Ads.txt
- ([Repo](https://github.com/FadeMind/hosts.extras)) :: https://raw.githubusercontent.com/FadeMind/hosts.extras/master/add.2o7Net/hosts
- ([Repo](https://github.com/crazy-max/WindowsSpyBlocker)) :: https://raw.githubusercontent.com/crazy-max/WindowsSpyBlocker/master/data/hosts/spy.txt
- ([Site](https://hostfiles.frogeye.fr/)) :: https://hostfiles.frogeye.fr/firstparty-trackers-hosts.txt

The [tracking-strict](https://raw.githubusercontent.com/rniemand/pihole-adlist/master/lists/tracking-strict.txt) list is comprised of the following sources:

- ([Site](https://hostfiles.frogeye.fr/)) https://hostfiles.frogeye.fr/multiparty-trackers-hosts.txt

## Malicious

The [malicious](https://raw.githubusercontent.com/rniemand/pihole-adlist/master/lists/malicious.txt) list is comprised of the following sources:

- ([Repo](https://github.com/DandelionSprout/adfilt)) :: https://raw.githubusercontent.com/DandelionSprout/adfilt/master/Alternate%20versions%20Anti-Malware%20List/AntiMalwareHosts.txt
- ([Site](https://osint.digitalside.it/)) :: https://osint.digitalside.it/Threat-Intel/lists/latestdomains.txt
- https://s3.amazonaws.com/lists.disconnect.me/simple_malvertising.txt
- https://v.firebog.net/hosts/Prigent-Crypto.txt
- ([Repo](https://github.com/FadeMind/hosts.extras)) :: https://raw.githubusercontent.com/FadeMind/hosts.extras/master/add.Risk/hosts
- ([Repo](https://bitbucket.org/ethanr/dns-blacklists/src/master/)) :: https://bitbucket.org/ethanr/dns-blacklists/raw/8575c9f96e5b4a1308f2f12394abd86d0927a4a0/bad_lists/Mandiant_APT1_Report_Appendix_D.txt
- ([Site](https://phishing.army/)) :: https://phishing.army/download/phishing_army_blocklist_extended.txt
- https://malware-filter.gitlab.io/malware-filter/phishing-filter-hosts.txt
- ([Repo](https://gitlab.com/quidsup/notrack-blocklists/)) :: https://gitlab.com/quidsup/notrack-blocklists/raw/master/notrack-malware.txt
- ([Repo](https://github.com/Spam404/lists)) :: https://raw.githubusercontent.com/Spam404/lists/master/main-blacklist.txt
- ([Repo](https://github.com/AssoEchap/stalkerware-indicators)) :: https://raw.githubusercontent.com/AssoEchap/stalkerware-indicators/master/generated/hosts
- ([Site](https://urlhaus.abuse.ch/)) :: https://urlhaus.abuse.ch/downloads/hostfile/

The [malicious-strict](https://raw.githubusercontent.com/rniemand/pihole-adlist/master/lists/malicious-strict.txt) list is comprised of the following sources:

- https://v.firebog.net/hosts/Prigent-Malware.txt

## Other

The [other](https://raw.githubusercontent.com/rniemand/pihole-adlist/master/lists/other.txt) list is comprised of the following sources:

- ([Site](https://zerodot1.gitlab.io/CoinBlockerListsWeb/)) :: https://zerodot1.gitlab.io/CoinBlockerLists/hosts_browser

The [other-strict](https://raw.githubusercontent.com/rniemand/pihole-adlist/master/lists/other-strict.txt) list is comprised of the following sources:

- Nothing, yet...
