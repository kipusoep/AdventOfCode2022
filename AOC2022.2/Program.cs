await Assignment1();
Console.WriteLine();
await Assignment2();

static async Task Assignment2()
{
    var lines = await File.ReadAllLinesAsync("input/assignment-1.txt");
    var gameRounds = lines.Select(x => new GameRound
    {
        HandShapeOpponent = (HandShapeOpponent)x[0],
        DesiredOutcome = (DesiredOutcome)x[2],
        HandShapeResponse = HandShapeResponse.None,
    }).ToList();
    var totalScore = gameRounds.Sum(x => x.Score);

    Console.WriteLine("Total score: {0}", totalScore);
}

static async Task Assignment1()
{
    var lines = await File.ReadAllLinesAsync("input/assignment-1.txt");
    var gameRounds = lines.Select(x => new GameRound
    {
        HandShapeOpponent = (HandShapeOpponent)x[0],
        HandShapeResponse = (HandShapeResponse)x[2],
    }).ToList();
    var totalScore = gameRounds.Sum(x => x.Score);

    Console.WriteLine("Total score: {0}", totalScore);
}

internal enum HandShapeOpponent
{
    None = '-',
    Rock = 'A',
    Paper = 'B',
    Scissors = 'C',
}

internal enum HandShapeResponse
{
    None = '-',
    Rock = 'X',
    Paper = 'Y',
    Scissors = 'Z',
}

internal enum DesiredOutcome
{
    None = '-',
    Lose = 'X',
    Draw = 'Y',
    Win = 'Z',
}

internal class GameRound
{
    public HandShapeOpponent HandShapeOpponent { get; init; }
    public HandShapeResponse HandShapeResponse { get; set; }
    public DesiredOutcome DesiredOutcome { get; init; }

    public int Score
    {
        get
        {
            if (HandShapeResponse == HandShapeResponse.None)
            {
                DetermineHandShapeResponse();
            }

            var score = HandShapeResponse switch
            {
                HandShapeResponse.Rock => 1,
                HandShapeResponse.Paper => 2,
                HandShapeResponse.Scissors => 3,
                _ => throw new ArgumentOutOfRangeException(),
            };
            score += Won ? 6 : Draw ? 3 : 0;
            return score;
        }
    }

    private void DetermineHandShapeResponse()
    {
        HandShapeResponse = DesiredOutcome switch
        {
            DesiredOutcome.Draw => Enum.Parse<HandShapeResponse>(HandShapeOpponent.ToString()),
            DesiredOutcome.Lose => HandShapeOpponent switch
            {
                HandShapeOpponent.Rock => HandShapeResponse.Scissors,
                HandShapeOpponent.Paper => HandShapeResponse.Rock,
                HandShapeOpponent.Scissors => HandShapeResponse.Paper,
                _ => throw new ArgumentOutOfRangeException(),
            },
            DesiredOutcome.Win => HandShapeOpponent switch
            {
                HandShapeOpponent.Rock => HandShapeResponse.Paper,
                HandShapeOpponent.Paper => HandShapeResponse.Scissors,
                HandShapeOpponent.Scissors => HandShapeResponse.Rock,
                _ => throw new ArgumentOutOfRangeException(),
            },
            _ => HandShapeResponse,
        };
    }

    private bool Won => HandShapeOpponent == HandShapeOpponent.Rock && HandShapeResponse == HandShapeResponse.Paper || HandShapeOpponent == HandShapeOpponent.Paper && HandShapeResponse == HandShapeResponse.Scissors || HandShapeOpponent == HandShapeOpponent.Scissors && HandShapeResponse == HandShapeResponse.Rock;

    private bool Draw => HandShapeOpponent == HandShapeOpponent.Rock && HandShapeResponse == HandShapeResponse.Rock || HandShapeOpponent == HandShapeOpponent.Paper && HandShapeResponse == HandShapeResponse.Paper || HandShapeOpponent == HandShapeOpponent.Scissors && HandShapeResponse == HandShapeResponse.Scissors;
}