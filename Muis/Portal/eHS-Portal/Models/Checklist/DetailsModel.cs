using eHS.Portal.Model;

namespace eHS.Portal.Models.Checklist
{
  public class DetailsModel
  {
    public ChecklistHistory Checklist { get; set; }
    public string Action { get; set; }
    public int LastVersion { get; set; }
    public int SchemeID { get; set; }
    public string SchemeText { get; set; }
  }
}