using System.Threading.Tasks;

public interface ISceneController
{
    Task LoadScene(string sceneId, bool fadeInBlack = false);
}
