using System.Diagnostics;

namespace Bài_cuối_kỳ
{
    public partial class Form1 : Form
    {

        TimeSpan time;
        PictureBox[] disks;
        Stack<PictureBox> disksRodA, disksRodB, disksRodC;
        const int Def_Y = 383;
        const int Disk_Height = 21;
        int DelayTime;
        int movStep;
        public Form1()
        {
            InitializeComponent();
            disks = new PictureBox[] { picDisk1, picDisk2, picDisk3, picDisk4, picDisk5, picDisk6, picDisk7, picDisk8 };
            disksRodA = new Stack<PictureBox>();
            disksRodB = new Stack<PictureBox>();
            disksRodC = new Stack<PictureBox>();
            movStep = 0;
        }
        public struct ThuTuc
        {
            public int N;
            public Stack<PictureBox> A;
            public Stack<PictureBox> B;
            public Stack<PictureBox> C;
        }
        public void LogWritter(Stack<PictureBox> I, Stack<PictureBox> O)
        {
            Char i, o;
            if (I == disksRodA)
                i = 'A';
            else if (I == disksRodB)
                i = 'B';
            else
                i = 'C';
            if (O == disksRodA)
                o = 'A';
            else if (O == disksRodB)
                o = 'B';
            else
                o = 'C';
        }

        public void Movement(Stack<PictureBox> rodSrc, Stack<PictureBox> rodDes)
        {
            int x = 0, y = 0;
            PictureBox DiskPop = new PictureBox();
            Point pntScr = new Point(x, y);

            DiskPop = rodSrc.Pop();
            pntScr = DiskPop.Location;
            x = pntScr.X;
            y = pntScr.Y;
            Application.DoEvents();

            // Nhấc lên
            for (; y > 50; y--)
            {
                DiskPop.Visible = false;
                DiskPop.Location = new Point(x, y);
                DiskPop.Visible = true;
                Application.DoEvents();
                Thread.Sleep(DelayTime);
            }

            int xPeek = 0, yPeek = 0;
            PictureBox tempPeek = new PictureBox();
            if (rodDes.Count != 0)
            {
                tempPeek = rodDes.Peek();
                xPeek = tempPeek.Location.X;
                yPeek = tempPeek.Location.Y - 20;
            }
            else
            {
                yPeek = Def_Y;
                if (rodDes == disksRodA)
                    xPeek = RodA.Location.X;
                else
                    if (rodDes == disksRodB)
                    xPeek = RodB.Location.X;
                else
                    xPeek = RodC.Location.X;
            }
            if (xPeek > x)
                for (; x <= xPeek; x++)
                {
                    DiskPop.Visible = false;
                    DiskPop.Location = new Point(x, y);
                    DiskPop.Visible = true;
                    Application.DoEvents();
                    Thread.Sleep(DelayTime);
                }
            else
                for (; x >= xPeek; x--)
                {
                    DiskPop.Visible = false;
                    DiskPop.Location = new Point(x, y);
                    DiskPop.Visible = true;
                    Application.DoEvents();
                    Thread.Sleep(DelayTime);
                }
            for (; y <= yPeek; y++)
            {
                DiskPop.Visible = false;
                DiskPop.Location = new Point(x, y);
                DiskPop.Visible = true;
                Application.DoEvents();
                Thread.Sleep(DelayTime);
            }
            rodDes.Push(DiskPop);
        }

        public void HNTByStack(int x)
        {

            ThuTuc X = new ThuTuc();
            X.N = x;
            X.A = disksRodA;
            X.C = disksRodB;
            X.B = disksRodC;

            Stack<ThuTuc> myStack = new Stack<ThuTuc>();
            ThuTuc temp = new ThuTuc();
            ThuTuc temp1 = new ThuTuc();
            myStack.Push(X);
            do
            {
                temp = myStack.Pop();
                if (temp.N == 1)
                {
                    movStep++;
                    LogWritter(temp.A, temp.B);
                    Movement(temp.A, temp.B);
                }
                else
                {
                    temp1.N = temp.N - 1;
                    temp1.A = temp.C;
                    temp1.B = temp.B;
                    temp1.C = temp.A;
                    myStack.Push(temp1);
                    temp1.N = 1;
                    temp1.A = temp.A;
                    temp1.B = temp.B;
                    temp1.C = temp.C;
                    myStack.Push(temp1);
                    temp1.N = temp.N - 1;
                    temp1.A = temp.A;
                    temp1.B = temp.C;
                    temp1.C = temp.B;
                    myStack.Push(temp1);
                }

            } while (myStack.Count != 0);
            timer1.Stop();
            MessageBox.Show("Task Completed after " + movStep + " steps!!!", "Program's Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Start_bnt.Enabled = true;
            Disk_Amount.Enabled = true;

        }

        public void InitalizeDisk()
        {
            timer1.Stop();
            foreach (PictureBox disk in disks)
                disk.Visible = false;
            time = new TimeSpan(0);
            Time_Counter.Text = "Time: 00:00:00";
            disksRodA.Clear();
            disksRodB.Clear();
            disksRodC.Clear();
            Disk_Amount.Enabled = false;

            int x = RodA.Location.X;
            int y = Def_Y;

            for (int i = (int)Disk_Amount.Value - 1; i >= 0; --i, y -= Disk_Height)
            {
                for (int j = 100; j <= y; j += 2)
                {
                    disks[i].Location = new Point(x, j);
                    disks[i].Visible = false;
                    disks[i].Visible = true;
                    Application.DoEvents();
                    Thread.Sleep(DelayTime);
                }
                disksRodA.Push(disks[i]);
            }
            timer1.Start();
        }

        private void Exit_Program_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có thoát ra không?", "Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void Start_bnt_Click(object sender, EventArgs e)
        {
            Start_bnt.Enabled = false;
            InitalizeDisk();
            HNTByStack((int)Disk_Amount.Value);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time = time.Add(new TimeSpan(0, 0, 1));
            Time_Counter.Text = string.Format("Time: {0:00}:{1:00}:{2:00}", time.Hours, time.Minutes, time.Seconds);
        }

        public void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnShowRule_Click(object sender, EventArgs e)
        {
                MessageBox.Show("Luật chơi:\n- Mỗi lần chỉ được di chuyển 1 đĩa trên cùng của cọc. \n- Đĩa nằm trên phải nhỏ hơn đĩa ở dưới", "Luật chơi", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictDisk1_Click(object sender, EventArgs e)
        {

        }

    }
}