using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StackExtensions
{
    // ������Ʈ Ǯ�� ������Ʈ�� �߰��ϴ� Ȯ�� �޼���
    public static void PushPool(this Stack<GameObject> stack, GameObject item)
    {
        item.gameObject.SetActive(false);
        stack.Push(item);
    }

    // ������Ʈ Ǯ���� ������Ʈ�� �������� Ȯ�� �޼���
    public static GameObject PopPool(this Stack<GameObject> stack)
    {
        if (stack.Count == 0)
        {
            Debug.LogWarning("!!!queue.Count == 0!!!");
            return null;
        }

        GameObject item = stack.Pop();
        item.gameObject.SetActive(true);
        return item;
    }

    public static GameObject PopPool(this Stack<GameObject> stack, Vector3 pos)
    {
        if (stack.Count == 0)
        {
            Debug.LogWarning("!!!queue.Count == 0!!!");
            return null;
        }

        GameObject item = stack.Pop();
        item.transform.position = pos;
        item.gameObject.SetActive(true);
        return item;
    }
}

public class Pool//Ǯ ���� Ŭ����
{
    public Stack<GameObject> stack;
    public int count;
    public Transform transform;

    public Pool(Transform transform)
    {
        stack = new Stack<GameObject>();
        count = 0;
        this.transform = transform;
    }
}

public class ObjectPoolManager : SceneSingleton<ObjectPoolManager>
{
    //������Ʈ�� Ǯ�� �������� ����� �� �ֵ��� �����ִ� �̱��� Ŭ����.
    private Dictionary<string, Pool> objectPools = new Dictionary<string, Pool>();
    //������ ������ Ÿ���� string���� �˻���.
    //�̸��� ������ ������ ���������� ����ϱ� ������ �̸� ������ ������ ��.

    public void CreatePool(GameObject prefab, int count = 2)//Ǯ�� count��ŭ ����.
    {
        string itemType = prefab.name;
        if (!objectPools.ContainsKey(itemType))//Ű�� ���� ���
        {
            //Ǯ�� ������ Ʈ�������� �߰��ϰ�, ������ Ǯ�� ��ųʸ��� �߰���.
            GameObject pool = new GameObject();
            pool.transform.SetParent(this.transform);
            pool.name = prefab.name + "Pool";
            objectPools.Add(itemType, new Pool(pool.transform));
        }

        for (int i = 0; i < count; i++)//count��ŭ ������ ������Ʈ�� �����ؼ� ��ť.
        {
            GameObject item = Instantiate(prefab, objectPools[itemType].transform);
            item.name = itemType;
            objectPools[itemType].stack.PushPool(item);
            objectPools[itemType].count++;
        }
    }
    public void EnqueueObject(GameObject item)//�� �� �� ť�� �ٽ� ����. Destroy�� ��ü��.
    {
        string itemType = item.name;
        if (!objectPools.ContainsKey(itemType))//Ű�� ���� ���
        {
            CreatePool(item);//�ڵ����� Ǯ�� ����
        }
        item.transform.SetParent(objectPools[itemType].transform);
        objectPools[itemType].stack.PushPool(item);
    }
    public void AllDestroyObject(GameObject prefab)//prefab�� ���� Ÿ���� ��� ������Ʈ�� ť�� �ٽ� ����.
    {
        string itemType = prefab.name;
        if (!objectPools.ContainsKey(itemType))//Ű�� ���� ���
        {
            CreatePool(prefab);//�ڵ����� Ǯ�� ����
        }

        for (int i = 0; i < objectPools[itemType].transform.childCount; i++)//Ű���� �ش��ϴ� Ʈ�������� ��� ������ �˻�
        {
            GameObject item = objectPools[itemType].transform.GetChild(i).gameObject;
            if (item.activeSelf)
            {
                EnqueueObject(item);//Ȱ��ȭ�Ǿ����� ��� ��ť.
            }
        }
    }

    public GameObject DequeueObject(GameObject prefab)//�� �� ��ȯ��. Instantiate�� ��ü��.
    {
        string itemType = prefab.name;
        if (!objectPools.ContainsKey(itemType))//Ű�� ���� ���
        {
            CreatePool(prefab);//�ڵ����� Ǯ�� ����
        }
        GameObject dequeneObject = objectPools[itemType].stack.PopPool();
        //��ť �õ�. ť�� �ִ� ��� �������� ������ϰ�� null�� ��ȯ��.
        if (dequeneObject != null)//ť�� ���빰�� ���� ���
        {
            //Debug.Log(objectPools[itemType].stack.Count);
            return dequeneObject;//�ش� ������Ʈ�� ��ȯ
        }
        else//ť�� ���빰�� ���� ���
        {
            CreatePool(prefab, objectPools[itemType].count);//Ǯ Ȯ��.
            return DequeueObject(prefab);//�߰��� Ǯ���� ��ť.
        }
    }
    public GameObject DequeueObject(GameObject prefab, Vector3 pos)//�� �� ��ȯ��. Instantiate�� ��ü��.
    {
        string itemType = prefab.name;
        if (!objectPools.ContainsKey(itemType))//Ű�� ���� ���
        {
            CreatePool(prefab);//�ڵ����� Ǯ�� ����
        }
        GameObject dequeneObject = objectPools[itemType].stack.PopPool(pos);
        //��ť �õ�. ť�� �ִ� ��� �������� ������ϰ�� null�� ��ȯ��.
        if (dequeneObject != null)//ť�� ���빰�� ���� ���
        {
            //Debug.Log(objectPools[itemType].stack.Count);
            return dequeneObject;//�ش� ������Ʈ�� ��ȯ
        }
        else//ť�� ���빰�� ���� ���
        {
            CreatePool(prefab, objectPools[itemType].count);//Ǯ Ȯ��.
            dequeneObject = objectPools[itemType].stack.PopPool(pos);
            return dequeneObject;
        }
    }

    /// <summary>
    /// �ð��� �����ؼ� ��ȯ��
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="time"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void EnqueueObject(GameObject item, float time)
    {
        StartCoroutine(DelayedEnqueu(item, time));
    }
    private IEnumerator DelayedEnqueu(GameObject item, float time)
    {

        Debug.Assert(item != null, "delayed returning pool item is null");
        yield return new WaitForSeconds(time);
        EnqueueObject(item);
    }
}