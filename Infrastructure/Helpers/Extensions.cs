using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Helpers {
  public static class Extensions {
    public static List<string> Shuffle(this List<string> inputList) {
      return inputList.OrderBy(a => Guid.NewGuid()).ToList();
    }
  }
}
