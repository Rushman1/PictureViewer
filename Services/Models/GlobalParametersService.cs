using Business;
using Infrastructure.Enums;
using Infrastructure.Interfaces;

namespace Services.Models {
  public class GlobalParametersService : IGlobalParametersService {
    public Settings GlobalSettings { get; set; }
    public ProgramStatesEnum ProgramState { get; set; }
  }
}
