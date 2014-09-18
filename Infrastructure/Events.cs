using System;
using System.Collections.Generic;
using Infrastructure.Enums;
using Microsoft.Practices.Prism.Events;

namespace Infrastructure {
  public class SetIsBusyEvent : CompositePresentationEvent<bool> { }
  public class SetBusyMessageEvent : CompositePresentationEvent<string> { }
  public class LoadMessageUpdateEvent : CompositePresentationEvent<string> { }
  public class SetProgramStateEvent : CompositePresentationEvent<ProgramStatesEnum> { }
  public class SetDebugMessageEvent : CompositePresentationEvent<List<String>> { }
  public class SetDebugWindowVisibilityEvent : CompositePresentationEvent<bool> {  }
}
