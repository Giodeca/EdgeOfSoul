using UnityEngine;

public class CamTest : ItemCollector
{
    #region Properties
    //[SerializeField] Camera SkyCamera;
    [SerializeField] Camera CameraTarget;
    [SerializeField] Photos _Photo;
    #endregion


    #region Additional Methods
    protected override void OnUse()
    {
        _Photo.PhotoGraph();
    }
    #endregion

}
