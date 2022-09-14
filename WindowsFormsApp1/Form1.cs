using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        const int prog = 0;
        const int sov_op = 1;
        const int op = 2;
        const int op_prisv = 3;
        const int op_usl = 4;
        const int op_cicl = 5;
        const int ar_vir = 6;
        const int E = 7;
        const int E_spis = 8;
        const int T = 9;
        const int T_spis = 10;
        const int F = 11;
        const int log_vir = 12;
        const int log_op = 13;
        const int assign = 14;
        const int semicolon = 15;
        const int lbracket = 16;
        const int rbracket = 17;
        const int Else = 18;
        const int Endif = 19;
        const int Until = 20;
        const int id = 21;
        const int end = 22;

        const int plus = 23;
        const int mult = 24;
        const int asgn = 25;
        const int ravn = 26;
        const int neravn = 27;
        const int metk = 28;
        const int po_nul = 29;
        const int bez_us = 30;

        const int L_spis = 31;

        public DataTable dt1, dt2;
        const int nmax = 70000;
        int ind = -1;
        int pt = -1, lb = -1;
        Stacks Com_d = new Stacks();
        Stacks Labels = new Stacks();
        int[] TLab = new int[100];
        int[] adr = new int[nmax];
        string[] datamas = new string[nmax];
        int[] datamean = new int[nmax];

        public Form1()
        {
            InitializeComponent();
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void ReadStr(string str)
        {
            Labels = new Stacks();
            pt = -1; lb = -1;
            ind = -1;
            Node lexem;
            int len, i = 0;
            len = str.Length;
            char[] mas = new char[len];
            for (int v = 0; v < nmax; v++)
            {
                datamas[v] = "";
                datamean[v] = -10;
                adr[v] = 0;
            }
            dt1 = new DataTable();
            dt2 = new DataTable();
            dt1.Clear();
            Com_d = new Stacks();

            dt1.Columns.Add("№", typeof(Int32));
            dt1.Columns.Add("Идент. и конст.", typeof(String));
            dt1.Columns.Add("Значение", typeof(Int32));

            dt2.Columns.Add("№", typeof(String));
            dt2.Columns.Add("Команда", typeof(String));

            Stacks St = new Stacks();
            mas = str.ToCharArray();
            lexem = GetLexem(str, ref i);

            St.Push(22);
            St.Push(0);
            try
            {
                Syntax(lexem, St, len, str, i);

                Translate(Com_d);
                if (checkBox1.Checked == false)
                    for (int j = 0; j < pt + 1; j++)
                    {
                        dt1.Rows.Add(j, datamas[j], datamean[j]);
                    }
            }
            catch (Exception e)
            {
                MessageBox.Show("Недопустимая цепочка.");
                dt1 = new DataTable();
                dt2 = new DataTable();
            }
            dataGridView1.DataSource = dt1;
            dataGridView2.DataSource = dt2;
            return;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string str;
            str = textBox1.Text;
            ReadStr(str);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public Node GetLexem(string arr, ref int i)
        {
            Node lexem = new Node(-1, "");
            while (i < arr.Length && (arr[i] == ' ' || arr[i] == '\n' || arr[i] == '\r'))
                i++;
            if (i==arr.Length)
            {
                lexem.el = (int)MyEnum.END;
                lexem.str = "END";
                i++;
                return lexem;
            }
            if (i > arr.Length)
            {
                return lexem;
            }
            
            string str = "";
            while (i < arr.Length && 'a' <= arr[i] && arr[i] <= 'z')
            {
                str += Convert.ToString(arr[i]);
                i++;
            }
            if (str != "" && i < arr.Length && '0' <= arr[i] && arr[i] <= '9')
            {
                return lexem;
            }
            lexem.str = str;
            if (str == "if")
            {
                lexem.el = (int)MyEnum.KEYWORD_IF;
            }
            else if (str == "else")
            {
                lexem.el = (int)MyEnum.KEYWORD_ELSE;
            }
            else if (str == "repeat")
            {
                lexem.el = (int)MyEnum.KEYWORD_REPEAT;
            }
            else if (str == "until")
            {
                lexem.el = (int)MyEnum.KEYWORD_UNTIL;
            }
            else if (str != "")
            {
                lexem.el = (int)MyEnum.IDENTIFIER;
            }
            else if ("*+;()=".Contains(arr[i]))
            {
                str = Convert.ToString(arr[i]);
                i++;
                lexem.str = str;
                switch (str)
                {
                    case "*":
                        lexem.el = (int)MyEnum.MULTIPLICATION;
                        break;
                    case "+":
                        lexem.el = (int)MyEnum.PLUS;
                        break;
                    case "=":
                        lexem.el = (int)MyEnum.EQUAL;
                        break;
                    case ";":
                        lexem.el = (int)MyEnum.SEMICOLON;
                        break;
                    case "(":
                        lexem.el = (int)MyEnum.LBRACKET;
                        break;
                    case ")":
                        lexem.el = (int)MyEnum.RBRACKET;
                        break;
                }
            }
            else if ('0' <= arr[i] && arr[i] <= '9')
            {
                while (i < arr.Length && '0' <= arr[i] && arr[i] <= '9')
                {
                    str += Convert.ToString(arr[i]);
                    i++;
                }
                if ('a' <= arr[i] && arr[i] <= 'z')
                {
                    return lexem;
                }
                if (str != "")
                {
                    lexem.str = str;
                    lexem.el = (int)MyEnum.INT;
                }
            }
            else if (i < arr.Length - 1 && (arr[i] == ':' || arr[i] == '!'))
            {
                str += Convert.ToString(arr[i]);
                i++;
                str += Convert.ToString(arr[i]);
                i++;
                switch (str)
                {
                    case "!=":
                        lexem.el = (int)MyEnum.DNT_EQUAL;
                        break;
                    case ":=":
                        lexem.el = (int)MyEnum.ASSIGN;
                        break;
                }
                if (lexem.el != -1)
                    lexem.str = str;
            }
            if (lexem.str == "")
                MessageBox.Show("введено неверное выражение");
            return lexem;    
        }

        public void Syntax(Node lexem, Stacks st, int n, string arr, int pos)
        {
            bool flag = true;
            int pt_prom;
            int pt1 = 0;
            int elem = lexem.el;
            int peek = st.Peek();
            switch (st.Peek())
            {
                case prog:
                    switch (elem)
                    {
                        case 0:
                            st.Pop();
                            st.Push(1);
                            break;
                        case 3:
                            st.Pop();
                            st.Push(1);
                            break;
                        case 7:
                            st.Pop();
                            st.Push(1);
                            break;
                        case 14:
                            st.Pop();
                            st.Push(1);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case sov_op:
                    switch (elem)
                    {
                        case 0:
                            st.Pop();
                            st.Push(1);
                            st.Push(2);
                            break;
                        case 3:
                            st.Pop();
                            st.Push(1);
                            st.Push(2);
                            break;
                        case 7:
                            st.Pop();
                            st.Push(1);
                            st.Push(2);
                            break;
                        case 8:
                            st.Pop();
                            break;
                        case 14:
                            st.Pop();
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case op:
                    switch (elem)
                    {
                        case 0:
                            st.Pop();
                            st.Push(3);
                            break;
                        case 3:
                            st.Pop();
                            st.Push(4);
                            break;
                        case 7:
                            st.Pop();
                            st.Push(5);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case op_prisv:
                    switch (elem)
                    {
                        case 0:
                            st.Pop();
                            st.Push(15);
                            st.Push(-1);
                            if (!Search(ref pt1, lexem.str))
                            {
                                pt++;
                                datamas[pt] = lexem.str;
                                st.Push(pt);
                            }
                            else
                            {
                                st.Push(pt1);
                            }
                            st.Push(asgn);
                            st.Push(st.size - 3);
                            st.Push(6);
                            st.Push(14);
                            lexem = GetLexem(arr, ref pos);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case op_usl:
                    switch (elem)
                    {
                        case 3:
                            st.Pop();
                            st.Push(++lb);
                            st.Push(19);
                            st.Push(2);
                            st.Push(lb);
                            st.Push(-1);
                            st.Push(po_nul);
                            st.Push(17);
                            st.Push(st.size - 3);
                            st.Push(12);
                            st.Push(16);
                            lexem = GetLexem(arr, ref pos);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case op_cicl:
                    switch (elem)
                    {
                        case 7:
                            st.Pop();
                            lb++;
                            st.Push(15);
                            st.Push(lb);
                            st.Push(-1);
                            st.Push(po_nul);
                            st.Push(st.size - 2);
                            st.Push(12);
                            st.Push(20);
                            st.Push(1);
                            st.Push(lb);
                            st.Push(metk);
                            lexem = GetLexem(arr, ref pos);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case ar_vir:
                    switch (elem)
                    {
                        case 0:
                            st.Pop();
                            st.Push(7);
                            break;
                        case 11:
                            st.Pop();
                            st.Push(7);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case E:
                    switch (elem)
                    {
                        case 0:
                            st.Pop();
                            st.Push(-1);
                            st.Push(8);
                            st.Push(st.size - 2);
                            st.Push(9);
                            break;
                        case 11:
                            st.Pop();
                            st.Push(-1);
                            st.Push(8);
                            st.Push(st.size - 2);
                            st.Push(9);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case E_spis:
                    switch (elem)
                    {
                        case 2:
                            st.Pop();
                            pt_prom = st.Pop();
                            st[st.Pop()] = pt_prom;
                            break;
                        case 9:
                            st.Pop();
                            pt_prom = st.Pop();
                            pt++;
                            st.Push(pt);
                            datamas[pt] = "new";
                            st.Push(8);
                            st.Push(pt);
                            st.Push(-1);
                            st.Push(pt_prom);
                            st.Push(plus);
                            st.Push(st.size - 3);
                            st.Push(9);
                            lexem = GetLexem(arr, ref pos);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case T:
                    switch (elem)
                    {
                        case 0:
                            st.Pop();
                            st.Push(-1);
                            st.Push(10);
                            st.Push(st.size - 2);
                            st.Push(11);
                            break;
                        case 11:
                            st.Pop();
                            st.Push(-1);
                            st.Push(10);
                            st.Push(st.size - 2);
                            st.Push(11);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case T_spis:
                    switch (elem)
                    {
                        case 2:
                            st.Pop();
                            pt_prom = st.Pop();
                            st[st.Pop()] = pt_prom;
                            break;
                        case 9:
                            st.Pop();
                            pt_prom = st.Pop();
                            st[st.Pop()] = pt_prom;
                            break;
                        case 10:
                            st.Pop();
                            pt_prom = st.Pop();
                            pt++;
                            st.Push(pt);
                            datamas[pt] = "new";
                            st.Push(10);
                            st.Push(pt);
                            st.Push(-1);
                            st.Push(pt_prom);
                            st.Push(mult);
                            st.Push(st.size - 3);
                            st.Push(11);
                            lexem = GetLexem(arr, ref pos);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case F:
                    switch (elem)
                    {
                        case 0:
                            st.Pop();
                            if (!Search(ref pt1, lexem.str))
                            {
                                pt++;
                                datamas[pt] = lexem.str;
                                st[st.Pop()] = pt;
                            }
                            else st[st.Pop()] = pt1;
                            lexem = GetLexem(arr, ref pos);
                            break;
                        case 11:
                            st.Pop();
                            if (!Search(ref pt1, lexem.str))
                            {
                                pt++;
                                datamas[pt] = lexem.str;
                                st[st.Pop()] = pt;
                                datamean[pt] = Convert.ToInt32(lexem.str);
                            }
                            else st[st.Pop()] = pt1;
                            lexem = GetLexem(arr, ref pos);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case L_spis:
                    switch (elem)
                    {
                        case 12:
                            st.Pop();
                            pt++;
                            datamas[pt] = "new";
                            pt_prom = st.Pop();
                            st[st.Pop()] = pt;
                            st.Push(pt);
                            st.Push(-1);
                            st.Push(pt_prom);
                            st.Push(ravn);
                            st.Push(st.size - 3);
                            st.Push(11);
                            lexem = GetLexem(arr, ref pos);
                            break;
                        case 13:
                            st.Pop();
                            pt++;
                            datamas[pt] = "new";
                            pt_prom = st.Pop();
                            st[st.Pop()] = pt;
                            st.Push(pt);
                            st.Push(-1);
                            st.Push(pt_prom);
                            st.Push(neravn);
                            st.Push(st.size - 3);
                            st.Push(11);
                            lexem = GetLexem(arr, ref pos);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case log_vir:
                    switch (elem)
                    {
                        case 0:
                            st.Pop();
                            st.Push(-1);
                            st.Push(31);
                            st.Push(st.size - 2);
                            st.Push(11);
                            break;
                        case 11:
                            st.Pop();
                            st.Push(-1);
                            st.Push(31);
                            st.Push(st.size - 2);
                            st.Push(11);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case log_op:
                    switch (elem)
                    {
                        case 12:
                            st.Pop();
                            lexem = GetLexem(arr, ref pos);
                            break;
                        case 13:
                            st.Pop();
                            lexem = GetLexem(arr, ref pos);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case assign:
                    switch (elem)
                    {
                        case 1:
                            st.Pop();
                            lexem = GetLexem(arr, ref pos);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case semicolon:
                    switch (elem)
                    {
                        case 2:
                            st.Pop();
                            lexem = GetLexem(arr, ref pos);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case lbracket:
                    switch (elem)
                    {
                        case 4:
                            st.Pop();
                            lexem = GetLexem(arr, ref pos);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case rbracket:
                    switch (elem)
                    {
                        case 5:
                            st.Pop();
                            lexem = GetLexem(arr, ref pos);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case Else:
                    switch (elem)
                    {
                        case 6:
                            st.Pop();
                            int pt = st.Pop();
                            lb++;
                            st.Push(lb);
                            st.Push(metk);
                            st.Push(2);
                            st.Push(pt);
                            st.Push(metk);
                            st.Push(lb);
                            st.Push(bez_us);
                            lexem = GetLexem(arr, ref pos);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case Endif:
                    switch (elem)
                    {
                        case 0:
                            st.Pop();
                            int pt = st.Pop();
                            st.Push(pt);
                            st.Push(metk);
                            break;
                        case 6:
                            st.Pop();
                            st.Push(18);
                            break;
                        case 14:
                            {
                                st.Pop();
                                pt = st.Pop();
                                st.Push(pt);
                                st.Push(metk);
                                break;
                            }
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case id:
                    switch (elem)
                    {
                        case 0:
                            st.Pop();
                            lexem = GetLexem(arr, ref pos);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case Until:
                    switch (elem)
                    {
                        case 8:
                            st.Pop();
                            lexem = GetLexem(arr, ref pos);
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                case end:
                    switch (elem)
                    {
                        case 14:
                            flag = true;
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    break;
                default:
                    {
                        if (st.Peek() >= 23 && st.Peek() <= 30)
                        {
                            Proverka_Command(lexem, st, n, arr, pos, st.Pop());
                        }
                        else flag = false;
                        break;
                    }
            }
            if (!flag || (lexem.el == -1))
            {
                throw new Exception();
                return;
            }
            else if ((flag) && (st.Peek() != 22))
            {
                Syntax(lexem, st, n, arr, pos);
            }
            else
            {
                return;
            }
            return;

        }

        bool Search(ref int pt1, string s)
        {
            for (int i = 0; i < nmax; i++)
            {
                if (datamas[i] == s)
                {
                    pt1 = i;
                    return true;
                }
            }
            return false;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        void Proverka_Command(Node lex, Stacks St, int n, string mas, int pos, int pt_prom)
        {
            switch (pt_prom)
            {
                case 23:
                    {
                        adr[++ind] = St.Pop();
                        Com_d.Push(ind);
                        Com_d.Push(plus);
                        adr[++ind] = St.Pop();
                        adr[++ind] = St.Pop();
                        Syntax(lex, St, n, mas, pos);
                        break;
                    }
                case 24:
                    {
                        adr[++ind] = St.Pop();
                        Com_d.Push(ind);
                        Com_d.Push(mult);
                        adr[++ind] = St.Pop();
                        adr[++ind] = St.Pop();
                        Syntax(lex, St, n, mas, pos);
                        break;
                    }
                case 25:
                    {
                        adr[++ind] = St.Pop();
                        Com_d.Push(ind);
                        Com_d.Push(asgn);
                        adr[++ind] = St.Pop();
                        Syntax(lex, St, n, mas, pos);
                        break;
                    }
                case 26:
                    {
                        adr[++ind] = St.Pop();
                        Com_d.Push(ind);
                        Com_d.Push(ravn);
                        adr[++ind] = St.Pop();
                        adr[++ind] = St.Pop();
                        Syntax(lex, St, n, mas, pos);
                        break;
                    }
                case 27:
                    {
                        adr[++ind] = St.Pop();
                        Com_d.Push(ind);
                        Com_d.Push(neravn);
                        adr[++ind] = St.Pop();
                        adr[++ind] = St.Pop();
                        Syntax(lex, St, n, mas, pos);
                        break;
                    }
                case 28:
                    {
                        adr[++ind] = St.Pop();
                        Labels.Push(Com_d.size);
                        TLab[adr[ind]] = Labels.size - 1;
                        Com_d.Push(ind);
                        Com_d.Push(metk);
                        Syntax(lex, St, n, mas, pos);
                        break;
                    }
                case 29:
                    {
                        adr[++ind] = St.Pop();
                        Com_d.Push(ind);
                        Com_d.Push(po_nul);
                        adr[++ind] = St.Pop();
                        Syntax(lex, St, n, mas, pos);
                        break;
                    }
                case 30:
                    {
                        adr[++ind] = St.Pop();
                        Com_d.Push(ind);
                        Com_d.Push(bez_us);
                        Syntax(lex, St, n, mas, pos);
                        break;
                    }
            }
        }

        void Translate(Stacks St)
        {
            int i = 0, j = 0;
            int p, q, r;
            int count;
            int ind1;
            while (i < St.size)
            {
                ind1 = St[i];
                count = St[++i];
                switch (count)
                {
                    case 23:
                        {
                            p = adr[ind1];
                            q = adr[++ind1];
                            r = adr[++ind1];
                            datamean[r] = datamean[p] + datamean[q];
                            if (checkBox1.Checked == true)
                                dt1.Rows.Add(r, datamas[r], datamean[r]);
                            dt2.Rows.Add(j.ToString(), "Сложение");
                            j++;
                            dt2.Rows.Add("", p.ToString());
                            dt2.Rows.Add("", q.ToString());
                            dt2.Rows.Add("", r.ToString());
                        }
                        break;
                    case 24:
                        {
                            p = adr[ind1];
                            q = adr[++ind1];
                            r = adr[++ind1];
                            datamean[r] = datamean[p] * datamean[q];
                            if (checkBox1.Checked == true)
                                dt1.Rows.Add(r, datamas[r], datamean[r]);
                            dt2.Rows.Add(j.ToString(), "Умножение");
                            j++;
                            dt2.Rows.Add("", p.ToString());
                            dt2.Rows.Add("", q.ToString());
                            dt2.Rows.Add("", r.ToString());
                        }
                        break;
                    case 25:
                        {
                            p = adr[ind1];
                            q = adr[++ind1];
                            datamean[p] = datamean[q];
                            if (checkBox1.Checked == true)
                                dt1.Rows.Add(p, datamas[p], datamean[p]);
                            dt2.Rows.Add(j.ToString(), "Присваивание");
                            j++;
                            dt2.Rows.Add("", p.ToString());
                            dt2.Rows.Add("", q.ToString());
                        }
                        break;
                    case 26:
                        {
                            p = adr[ind1];
                            q = adr[++ind1];
                            r = adr[++ind1];
                            if (datamean[p] == datamean[q])
                                datamean[r] = 1;
                            else datamean[r] = 0;
                            if (checkBox1.Checked == true)
                                dt1.Rows.Add(r, datamas[r], datamean[r]);
                            dt2.Rows.Add(j.ToString(), "Равно");
                            j++;
                            dt2.Rows.Add("", p.ToString());
                            dt2.Rows.Add("", q.ToString());
                            dt2.Rows.Add("", r.ToString());
                        }
                        break;
                    case 27:
                        {
                            p = adr[ind1];
                            q = adr[++ind1];
                            r = adr[++ind1];
                            if (datamean[p] != datamean[q])
                                datamean[r] = 1;
                            else datamean[r] = 0;
                            if (checkBox1.Checked == true)
                                dt1.Rows.Add(r, datamas[r], datamean[r]);
                            dt2.Rows.Add(j.ToString(), "Не равно");
                            j++;
                            dt2.Rows.Add("", p.ToString());
                            dt2.Rows.Add("", q.ToString());
                            dt2.Rows.Add("", r.ToString());
                        }
                        break;
                    case 28:
                        {
                            p = adr[ind1];
                            dt2.Rows.Add(j.ToString(), "Метка");
                            j++;
                            dt2.Rows.Add("", p.ToString());
                        }
                        break;
                    case 29:
                        {
                            p = adr[ind1];
                            q = adr[++ind1];
                            if (datamean[p] == 0)
                            {
                                i = Labels[TLab[q]] - 1;
                            }
                            dt2.Rows.Add(j.ToString(), "Условный переход по 0");
                            j++;
                            dt2.Rows.Add("", p.ToString());
                            dt2.Rows.Add("", q.ToString());
                        }
                        break;
                    case 30:
                        {
                            p = adr[ind1];
                            i = Labels[TLab[p]] - 1;
                            dt2.Rows.Add(j.ToString(), "Безусловный переход");
                            j++;
                            dt2.Rows.Add("", p.ToString());
                        }
                        break;
                }
                i++;
            }
            return;
        }
    }


}
/* 0 - if
 * 1 - else
 * 2 - while
 * 3 - */
