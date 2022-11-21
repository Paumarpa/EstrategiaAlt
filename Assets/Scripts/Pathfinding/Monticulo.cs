using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Monticulo<T> where T : IHeapItem<T>
{
    T[] items;
    int itemCount;

    public Monticulo(int tamañoMax)
    {
        items = new T[tamañoMax];
    }

    public void Add(T item)
    {
        item.HeapIndex = itemCount;
        items[itemCount] = item;
        Ordena(item);
        itemCount++;
    }

    public T EliminaItem()
    {
        T primerItem = items[0];
        itemCount--;
        items[0] = items[itemCount];
        items[0].HeapIndex = 0;
        OrdenaEliminado(items[0]);
        return primerItem;
    }

    public void update(T item)
    {
        Ordena(item);
    }
    public int Count
    {
        get
        {
            return itemCount;
        }
       
        
    }
    public bool Contains (T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    void OrdenaEliminado(T item)
    {
        while (true)
        {
            int hijoIzq = item.HeapIndex * 2 + 1;
            int hijoDer = item.HeapIndex * 2 + 2;

            int cambiarIndex = 0;

            if (hijoIzq < itemCount)
            {
                cambiarIndex = hijoIzq;

                if (hijoDer < itemCount)
                {
                    if (items[hijoIzq].CompareTo(items[hijoDer]) < 0)
                    {
                        cambiarIndex = hijoDer;
                    }
                }

                if (item.CompareTo(items[cambiarIndex]) < 0)
                {
                    Intercambia(item, items[cambiarIndex]);
                }
                else
                {
                    return;
                }

            }
            else
            {
                return;
            }
        }
    }
    void Ordena(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Intercambia(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Intercambia(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }

}


public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { 
        get;
        set;
    }
          
}
