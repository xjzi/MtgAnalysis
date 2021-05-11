public class Card
{
	public string Name { get; set; }
	public double Frequency { get; set; }
}
public class SimilarCards
{
	public Card A { get; set; }
	public Card B { get; set; }
	public int Distance { get; set; }
}