namespace AIMAS.Data
{
  interface IAimasDbModelWithUpdate<Model> : IAimasDbModel<Model>
  {
    void UpdateDb(Model model, AimasContext aimas);
  }
}
