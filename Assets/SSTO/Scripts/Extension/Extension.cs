using EnumTypes;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.ComponentModel;

public static class Extension
{
    //public static Node FindClosest(this List<Node> list, Vector3 position)
    //{
    //    if (list == null || list.Count == 0)
    //    {
    //        Debug.LogWarning("The transforms list is empty or null.");
    //        return null;
    //    }

    //    Node closestTransform = null;
    //    float closestDistanceSqr = Mathf.Infinity;

    //    foreach (Node node in list)
    //    {
    //        float distanceSqr = (node.gridPos - position).sqrMagnitude;

    //        if (distanceSqr < closestDistanceSqr)
    //        {
    //            closestDistanceSqr = distanceSqr;
    //            closestTransform = node;
    //        }
    //    }

    //    return closestTransform;
    //}

    //public static void InsertionCellSort<T>(this List<T> list) where T : CellData, IComparable<T>
    //{
    //    for (int i = 1; i < list.Count; i++)
    //    {
    //        T key = list[i];
    //        int j = i - 1;

    //        // �̵��� ��ġ�� ã���ϴ�
    //        while (j >= 0 && list[j].CompareTo(key) > 0)
    //        {
    //            j--;
    //        }

    //        // j+1�� key�� ���� ��ġ�Դϴ�.
    //        // ��ġ�� i�� �ٸ� ��쿡�� ��ȯ�� �����մϴ�.
    //        if (j + 1 != i)
    //        {
    //            list[j + 1].Swap_OnSort(list[i]);                
    //        }
    //    }
    //}

    public class VM
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)//���� ����Ǿ��� �� �̺�Ʈ�� �߻���Ű�� ���� �뵵 (������ ���ε�)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static void SetUIPos_WorldToScreenPos(this RectTransform rectTransform, Vector3 originPos)
    {
        Camera cam = GetHighestPriorityCamera();

        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector3 screenPosition = cam.WorldToScreenPoint(originPos);
        bool isOutsideOfCamera = (screenPosition.z < 0);// ||
                                                        //screenPosition.x < 0 || screenPosition.x > screenSize.x ||
                                                        //screenPosition.y < 0 || screenPosition.y > screenSize.y);

        if (isOutsideOfCamera)
        {
            rectTransform.anchoredPosition = new Vector2(-3000, -3000);
        }
        else
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, originPos);
            Vector2 position = screenPoint - screenSize * 0.5f;
            rectTransform.anchoredPosition = position;
        }
    }
    public static void ClampToScreen(this RectTransform rectTransform)
    {
        // ȭ�� ũ�� ��������
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // RectTransform�� �θ� ĵ���������� ��ġ�� ���
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        // RectTransform�� ���� ��Ŀ�� ������ ��������
        Vector2 anchoredPosition = rectTransform.anchoredPosition;

        // X ��ǥ Ŭ���� (���� �� �������� ȭ�� ������ ������ ���� ����)
        if (corners[0].x < 0) // RectTransform�� ������ ȭ�� ������ ���� ���
        {
            anchoredPosition.x += 0 - corners[0].x;
        }
        else if (corners[2].x > screenWidth) // RectTransform�� �������� ȭ�� ������ ���� ���
        {
            anchoredPosition.x -= corners[2].x - screenWidth;
        }

        // Y ��ǥ Ŭ���� (���� �� �Ʒ����� ȭ�� ������ ������ ���� ����)
        if (corners[0].y < 0) // RectTransform�� �Ʒ����� ȭ�� ������ ���� ���
        {
            anchoredPosition.y += 0 - corners[0].y;
        }
        else if (corners[1].y > screenHeight) // RectTransform�� ������ ȭ�� ������ ���� ���
        {
            anchoredPosition.y -= corners[1].y - screenHeight;
        }

        // Ŭ���ε� ��Ŀ�� �������� �ٽ� ����
        rectTransform.anchoredPosition = anchoredPosition;
    }

    public static Camera GetHighestPriorityCamera()
    {
        Camera[] allCameras = Camera.allCameras;
        Camera highestPriorityCamera = null;
        float maxDepth = float.MinValue;

        foreach (Camera cam in allCameras)
        {
            if (cam.depth > maxDepth)
            {
                maxDepth = cam.depth;
                highestPriorityCamera = cam;
            }
        }

        return highestPriorityCamera;
    }

    public static string GetTextTable(this string id, Language languageType = Language.Kr)
    {
        return JsonDataManager.GetText(id, languageType);
    }
    public static void SetLoadSprite(this Image image, string path)
    {
        // Resources.Load�� ����Ͽ� ��������Ʈ�� �ε��մϴ�.
        Sprite lodeSprite = Resources.Load<Sprite>(path);
        image.sprite = lodeSprite;
    }
    public static string Replace_ToItem(this string buildingId)
    {
        return buildingId.Replace("Building_", "Item_");
    }

    public static string SimplifyNumber(this float number)
    {
        if (number >= 1_000_000_000_000_000_000)
        {
            return $"{number / 1_000_000_000_000_000_000.0:F1}E";
        }
        else if (number >= 1_000_000_000_000_000)
        {
            return $"{number / 1_000_000_000_000_000.0:F1}P";
        }
        else if (number >= 1_000_000_000_000)
        {
            return $"{number / 1_000_000_000_000.0:F1}T";
        }
        else if (number >= 1_000_000_000)
        {
            return $"{number / 1_000_000_000.0:F1}G";
        }
        else if (number >= 1_000_000)
        {
            return $"{number / 1_000_000.0:F1}M";
        }
        else if (number >= 1_000)
        {
            return $"{number / 1_000.0:F1}k";
        }
        else
        {
            return $"{number:F0}";
        }
    }
}