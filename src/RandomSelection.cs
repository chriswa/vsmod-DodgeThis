namespace DodgeThis {
  public class ChanceAndResultPair<T> {
    public double Chance;
    public T Value;
  }
  public class RandomSelection<T> {
    public ChanceAndResultPair<T>[] Selections = new ChanceAndResultPair<T>[0];
    public T Select(double roll) {
      foreach (var chanceAndResultPair in Selections) {
        roll -= chanceAndResultPair.Chance;
        if (roll <= 0) {
          return chanceAndResultPair.Value;
        }
      }
      // should probably throw an exception here instead...
      return Selections[Selections.Length - 1].Value;
    }
  }
}