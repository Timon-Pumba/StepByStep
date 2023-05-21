﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Chapter_19_Wyliczanie_Kolekcji.Model
{
    internal class TreeEnumerator<TItem> : IEnumerator<TItem> where TItem : IComparable<TItem>
    {
        // W tym przypadku aby zapewnić iteracje po drzewie, wszystkie dane z drzewa zostaną przepisane do osobnej kolekcji Queue.

        private TItem currentItem = default(TItem); // Inicjiowanie_Zmiennej_Zdefiniowanej_Przy_Uzyciu_Parametru_Typu()
        private readonly Tree<TItem> currentData = null;
        private Queue<TItem> enumerableData = null;

        internal TreeEnumerator(Tree<TItem> data)
        {
            currentData = data;
        }

        /* Jawnie (explicity) implementowany interfejs. Wiecej o implementacjach interfejsów w rozdziale 13.
         * Znika modyfikator dostępu, ponieważ pole bedzie tylko widoczne gdy przypiszemy referencje obiektu do typu odpowiedniego interfejsu.
         * Jeśli kolekcja zwróci ten modul wyliczenowy o typie generycznym to mamy pewność, że zostanie użyty ten Current a nie typu object poniżej.
         */
        TItem IEnumerator<TItem>.Current
        {
            get
            {
                if (this.enumerableData == null)
                {
                    throw new InvalidOperationException("Use MoveNext before calling Current.");
                }

                return this.currentItem;
            }
        }

        // Generyczny IEnumerator<T> dziedziczy po swoim starszym bracie IEnumerator dlatego nalezy dodac implementacje obydwóch interfejsow. 
        // IEnumerator<T> posiada tylko właściwość Current z parametrem typu.
        object IEnumerator.Current => throw new NotImplementedException();


        // Zadaniem metody 'MoveNext' jest sprawdzenie czy istnieje kolejny element w kolekcji oraz ustawienie Currenta.
        public bool MoveNext()
        {
            // Na początku inicjiujemy kolekcje (mozna by było to przenieść do konstruktora)
            if (this.enumerableData == null)
            {
                this.enumerableData = new Queue<TItem>();
                PopulateQueue(this.currentData);
            }

            // Sprawdzam czy istnieje kolejny element, przypisuje do currentItem i zwracam wartość logiczną.
            if (this.enumerableData.Count > 0)
            {
                this.currentItem = this.enumerableData.Dequeue();
                // metoda Dequeue zwróci i usunie element z kolejki. (Przykład dobrego użycia odpowieniej kolekcji. Dzięki temu nie musze martwić się o index listy)

                return true;
            }

            return false;
        }

        // Zadaniem metody Reset jest ustawienie wskaźnika przed pierwszym elementem listy.       
        public void Reset()
        {
            /* W naszym przypadku nie musi być on zaimplementowany bo pętla foreach za każdym razem woła nowego enumeratora
             * a przechodzenie po kolekcji odbywa sie za pomoca Dequeue i podczas pierwszej iteracji MoveNext Kolejka zostanie wypelniona.
             */
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        private void PopulateQueue(Tree<TItem> tree)
        {
            if (tree.LeftTree != null)
            {
                PopulateQueue(tree.LeftTree);
            }
            this.enumerableData.Enqueue(tree.NodeData);

            if (tree.RightTree != null)
            {
                PopulateQueue(tree.RightTree);
            }
        }
    }
}
