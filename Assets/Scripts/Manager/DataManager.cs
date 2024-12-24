public class DataManager : Singleton<DataManager>
{
	private GameData gameData;
	public GameData GameData { get { if (gameData == null) { gameData = new GameData(); } return gameData; } }
}
