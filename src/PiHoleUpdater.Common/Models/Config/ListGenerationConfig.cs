using YamlDotNet.Serialization;

namespace PiHoleUpdater.Common.Models.Config;

public class ListGenerationConfig
{
  [YamlMember(Alias = "category_lists")]
  public bool GenerateCategoryLists { get; set; }

  [YamlMember(Alias = "combined_lists")]
  public bool GenerateCombinedLists { get; set; }
}
