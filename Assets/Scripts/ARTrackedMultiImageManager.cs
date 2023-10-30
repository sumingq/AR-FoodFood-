using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTrackedMultiImageManager : MonoBehaviour
{
    [SerializeField]
    // �̹����� �ν����� �� ��µǴ� ������ ���
    private GameObject[] trackedPrefabs; 

    // �̹����� �ν����� �� ��µǴ� ������Ʈ ���
    // Ű(�̹����� �̸�)�� ��(GameObject)�� ���� ����
    private Dictionary<string,GameObject> spawnedObject = new Dictionary<string, GameObject>();
    private ARTrackedImageManager trackedImageManager;

    private void Awake()
    {
        // AR Session Origin ������Ʈ�� ������Ʈ�� �������� �� ��� ����
        trackedImageManager = GetComponent<ARTrackedImageManager>();

        // trackedPrefabs �迭�� �ִ� ��� �������� Instantiate()�� ������ ��
        // spawnedObject dictionary�� ����, ��Ȱ��ȭ
        // ī�޶� �̹����� �νĵǸ� �̹����� ������ �̸��� key�� �ִ� value ������Ʈ�� ���
        foreach (GameObject prefab in trackedPrefabs)
        {
            GameObject clone = Instantiate(prefab); // ������Ʈ ����
            clone.name = prefab.name; // ������ ������Ʈ�� �̸� ����
            clone.SetActive(false); // ������Ʈ ��Ȱ��ȭ
            spawnedObject.Add(clone.name, clone); // Dictionary �ڷᱸ���� ������Ʈ ����
        }
    }

    // ARTrackedImageManager�� �̺�Ʈ �ڵ鷯�� ��� �� ����
    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    //  AR �̹��� Ʈ��ŷ ���� �̺�Ʈ�� �߻����� �� ȣ���
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // ī�޶� �̹����� �νĵ��� ��
        foreach(var trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }
        // ī�޶� �̹����� �νĵǾ� ������Ʈ�ǰ� ���� ��
        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }
        // �νĵǰ� �ִ� �̹����� ī�޶󿡼� ������� ��
        foreach (var trackedImage in eventArgs.removed)
        {
            spawnedObject[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        GameObject trackedObject = spawnedObject[name];

        // �̹����� ���� ���°� ������(Tracking) �� ��
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            trackedObject.transform.position = trackedImage.transform.position;
            trackedObject.transform.rotation = trackedImage.transform.rotation;
            trackedObject.SetActive(true);
        }
        else
        {
            trackedObject.SetActive(false);
        }
    }
}
