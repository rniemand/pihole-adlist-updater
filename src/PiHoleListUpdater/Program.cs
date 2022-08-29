using PiHoleListUpdater;

var adLists = new[]
{
  "https://adaway.org/hosts.txt",
  "https://v.firebog.net/hosts/AdguardDNS.txt",
  "https://v.firebog.net/hosts/Admiral.txt",
  "https://raw.githubusercontent.com/anudeepND/blacklist/master/adservers.txt",
  "https://s3.amazonaws.com/lists.disconnect.me/simple_ad.txt",
  "https://v.firebog.net/hosts/Easylist.txt",
  "https://pgl.yoyo.org/adservers/serverlist.php?hostformat=hosts&showintro=0&mimetype=plaintext",
  "https://raw.githubusercontent.com/FadeMind/hosts.extras/master/UncheckyAds/hosts",
  "https://raw.githubusercontent.com/bigdargon/hostsVN/master/hosts",
  "https://v.firebog.net/hosts/Easyprivacy.txt",
  "https://v.firebog.net/hosts/Prigent-Ads.txt",
  "https://raw.githubusercontent.com/FadeMind/hosts.extras/master/add.2o7Net/hosts",
  "https://raw.githubusercontent.com/crazy-max/WindowsSpyBlocker/master/data/hosts/spy.txt",
  "https://hostfiles.frogeye.fr/firstparty-trackers-hosts.txt",
  "https://raw.githubusercontent.com/DandelionSprout/adfilt/master/Alternate%20versions%20Anti-Malware%20List/AntiMalwareHosts.txt",
  "https://osint.digitalside.it/Threat-Intel/lists/latestdomains.txt",
  "https://s3.amazonaws.com/lists.disconnect.me/simple_malvertising.txt",
  "https://v.firebog.net/hosts/Prigent-Crypto.txt",
  "https://raw.githubusercontent.com/FadeMind/hosts.extras/master/add.Risk/hosts",
  "https://bitbucket.org/ethanr/dns-blacklists/raw/8575c9f96e5b4a1308f2f12394abd86d0927a4a0/bad_lists/Mandiant_APT1_Report_Appendix_D.txt",
  "https://phishing.army/download/phishing_army_blocklist_extended.txt",
  "https://malware-filter.gitlab.io/malware-filter/phishing-filter-hosts.txt",
  "https://gitlab.com/quidsup/notrack-blocklists/raw/master/notrack-malware.txt",
  "https://raw.githubusercontent.com/Spam404/lists/master/main-blacklist.txt",
  "https://raw.githubusercontent.com/AssoEchap/stalkerware-indicators/master/generated/hosts",
  "https://urlhaus.abuse.ch/downloads/hostfile/",
  "https://raw.githubusercontent.com/StevenBlack/hosts/master/hosts",
  "https://raw.githubusercontent.com/PolishFiltersTeam/KADhosts/master/KADhosts.txt",
  "https://raw.githubusercontent.com/FadeMind/hosts.extras/master/add.Spam/hosts",
  "https://v.firebog.net/hosts/static/w3kbl.txt",
  "https://zerodot1.gitlab.io/CoinBlockerLists/hosts_browser",
  "https://www.github.developerdan.com/hosts/lists/facebook-extended.txt",
  "https://www.github.developerdan.com/hosts/lists/ads-and-tracking-extended.txt"
};


var webService = new WebService();
var listParser = new AdListParser();
var domains = new HashSet<string>();
const string outFilePath = "C:\\WRK\\pihole-adlist\\lists\\everything.txt";

foreach (var adList in adLists)
{
  var rawAdList = await webService.GetUrContentAsync(adList);
  listParser.ParseList(domains, rawAdList);
}

var sortedDomains = domains
  .OrderBy(x => x)
  .ToArray();

if(File.Exists(outFilePath))
  File.Delete(outFilePath);

File.WriteAllText(outFilePath, string.Join("\r\n", domains));

Console.WriteLine(sortedDomains.Length);
