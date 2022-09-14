using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;

namespace WindowsFormsApp1
{
    public class Node
    {
        public int el;
        public string str;
        public Node next;

        public Node(int a, string s)
        {
            el = a;
            str = s;
            next = null;
        }
    }

    public class List
    {
        public Node head, cur;

        public bool Search(string s)
        {
            Node n = head;
            bool f = false;
            while (n != null && f == false)
            {
                if (n.str == s)
                {
                    f = true;
                }
                n = n.next;
            }
            return f;
        }

        public void Addl(int a, string s)
        {
            if (!Search(s))
            {
                if (head == null)
                {
                    head = new Node(a, s);
                    cur = head;
                }
                else
                {
                    Node n = new Node(a, s);
                    cur.next = n;
                    cur = cur.next;
                    cur.next = null;
                }
            }
            return;
        }

        public bool Del(string s)
        {
            Node n = head;
            bool flag = false;
            if (n != null)
            {
                if (n.str == s)
                {
                    head = head.next;
                    n = null;
                    flag = true;
                }
                else
                {
                    while (n.next.str != s && n != null)
                    {
                        n = n.next;
                    }
                    if (n != null)
                    {
                        Node d = n.next;
                        n.next = n.next.next;
                        d = null;
                        flag = true;
                    }
                }
            }
            return flag;
        }

        public List()
        {
            head = null;
            cur = head;
        }

        public List(int a, string s)
        {
            head = new Node(a, s);
            cur = head;
        }
    }

    public class Table
    {
        public List[] mas;
        public int len;

        public static int Hesh(string s, int len)
        {
            byte[] r;
            r = Encoding.ASCII.GetBytes(s);
            int ret = 0;
            for (int i = 0; i < s.Length; i++)
            {
                ret += r[i] * (i + 1);
            }
            ret %= len;
            return ret;
        }

        public Table(int s)
        {
            mas = new List[s];
            len = s;
        }

        public Table()
        {
            len = 12;
            mas = new List[len];
            for (int i = 0; i < len; i++)
            {
                mas[i] = new List();
            }
        }

        public void Add(Node n)
        {
            mas[Hesh(n.str, len)].Addl(n.el, n.str);
        }

        public void Print(ref DataTable dt)
        {
            for (int i = 0; i < len; i++)
            {
                Node n = mas[i].head;
                while (n != null)
                {

                    if (n.str != "" && n.el != -1)
                    {
                        dt.Rows.Add(n.str, n.el);
                        n = n.next;
                    }
                }
            }
        }
    }
    public class Stacks
    {
        public int[] mas;
        public int size;
        int n = 100;

        public Stacks()
        {
            mas = new int[n];
        }

        public bool IsEmpty()
        {
            return size == 0;
        }

        public int Size()
        {
            return size;
        }

        public void Push(int elem)
        {
            if (size == mas.Length)
                Array.Resize(ref mas, mas.Length + 10);
            mas[size++] = elem;
            return;
        }

        public int Pop()
        {
            if (IsEmpty())
                return -1;
            int elem = mas[--size];
            mas[size] = default(int);
            if (size > 0 && size < mas.Length - 10)
                Array.Resize(ref mas, mas.Length - 10);
            return elem;
        }

        public int this[int ind]
        {
            get
            {
                return mas[ind];
            }
            set
            {
                mas[ind] = value;
            }
        }

        public int Peek()
        {
            return mas[size - 1];
        }
        static class Program
        {
            /// <summary>
            /// Главная точка входа для приложения.
            /// </summary>
            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }

    }
}
