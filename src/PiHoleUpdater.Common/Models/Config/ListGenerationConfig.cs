using YamlDotNet.Serialization;

namespace PiHoleUpdater.Common.Models.Config;

public class ListGenerationConfig
{
  [YamlMember(Alias = "category_all")]
  public bool CategoryAll { get; set; }

  [YamlMember(Alias = "category_safe")]
  public bool CategorySafe { get; set; }

  [YamlMember(Alias = "combined_all")]
  public bool CombinedAll { get; set; }

  [YamlMember(Alias = "combined_safe")]
  public bool CombinedSafe { get; set; }

  [YamlMember(Alias = "generate_category_lists")]
  public bool GenerateCategoryLists { get; set; }

  [YamlMember(Alias = "generate_combined_lists")]
  public bool GenerateCombinedLists { get; set; }
}
