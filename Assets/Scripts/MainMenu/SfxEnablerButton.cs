using ParkourNews.Scripts;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SfxEnablerButton : MonoBehaviour
{
    [SerializeField] public Sprite offSprite;
    [SerializeField] public Sprite onSprite;
    
    private DataManager _dataManager;
    private AudioManager _audioManager;
    private Button _button;
    
    private bool _enabled;
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        
        _dataManager = FindObjectOfType<DataManager>();
        _audioManager = FindObjectOfType<AudioManager>();
        
        StartCoroutine(nameof(WaitForDataManager));
    }
    
    IEnumerator WaitForDataManager()
    {
        yield return new WaitForSeconds(0.2f);
        _enabled = _dataManager.GetSfxEnabled();
        ChangeImage();
    }

    private void OnClick()
    {
        _enabled = !_enabled;
        _audioManager.SetSfxEnabled(_enabled);
        ChangeImage();
    }

    private void ChangeImage()
    {
        _image.sprite = _enabled ? onSprite : offSprite;
    }
}