using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HuffmanTree
{
    class DrawTree
    {
        private Dictionary<char, double> tempDic;
        private TreeNode root;
        private List<TreeNode> nodes;
        private Dictionary<int, Canvas> canvasDic;
        private Dictionary<int, TreeNode> nodeDic;
        private int high = 0;
        private Canvas canvas1;
        private Label labelFamily;

        private int nodeLeft = 20;
        private int nodeTop = 20;
        public DrawTree(Canvas canvas,Label label)
        {
            tempDic = new Dictionary<char, double>();
            canvasDic = new Dictionary<int, Canvas>();
            nodeDic = new Dictionary<int, TreeNode>();
            this.canvas1 = canvas;
            nodeLeft = (int)canvas1.Width / 2 - 50;
            this.labelFamily = label;
        }
        public void creat_Tree(Dictionary<char, double> dic)
        {
            tempDic = dic;
            tempDic = tempDic.OrderBy(r => r.Value).ToDictionary(r => r.Key, r => r.Value);
            init_Nodes();
            creat_TreeList();

        }
        private void creat_TreeList()
        {
            int id = 1;
            while (nodes.Count > 1)
            {
                TreeNode subNode = new TreeNode();
                //建立huffman二叉树 同时为节点编序号
                subNode.leftChild = nodes.ElementAt(0);
                if (subNode.leftChild.id == 0)
                {
                    subNode.leftChild.id = id;
                    id++;
                    nodeDic.Add(id, subNode.leftChild);
                }

                subNode.rightChild = nodes.ElementAt(1);
                if (subNode.rightChild.id == 0)
                {
                    subNode.rightChild.id = id;
                    id++;
                    nodeDic.Add(id, subNode.rightChild);
                }
                subNode.weight = subNode.leftChild.weight + subNode.rightChild.weight;
                subNode.id = id;
                id++;
                nodeDic.Add(id, subNode);
                nodes.RemoveAt(1);
                nodes.RemoveAt(0);
                nodes.Add(subNode);
                nodes = nodes.OrderBy(r => r.weight).ToList();
            }
            try
            {
                this.root = nodes.ElementAt(0);
                root.id = id;
                //nodeDic.Add(id, root);
                root.x = nodeLeft;
                root.y = nodeTop;
            }
            catch
            {

            }
            
           
            high = get_High(root);
            //大于5层画不下
            if (high > 5)
            {
                return;
            }
            //遍历二叉树 同时设置每个节点的坐标
            pre_Order(root, high - 1);

            canvasDic = canvasDic.OrderBy(r => r.Key).ToDictionary(r => r.Key, r => r.Value);
            nodeDic = nodeDic.OrderBy(r => r.Key).ToDictionary(r => r.Key, r => r.Value);
            //每500毫秒 画节点画线
            Thread anotherThread = new Thread(() =>
            {
                for (int i = 0; i < canvasDic.Count; i++)
                {
                    canvas1.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        canvasDic.ElementAt(i).Value.Visibility = Visibility.Visible;
                        if (nodeDic.ElementAt(i).Value.leftChild != null)
                        {
                            draw_LeftLine(nodeDic.ElementAt(i).Value);
                        }
                        if (nodeDic.ElementAt(i).Value.rightChild != null)
                        {
                            draw_RightLine(nodeDic.ElementAt(i).Value);
                        }
                    }
                    ));
                    Thread.Sleep(500);
                }
            }
            );
            anotherThread.SetApartmentState(ApartmentState.STA);
            anotherThread.Start();

        }
        private void pre_Order(TreeNode subNode, int i)
        {
            if (subNode != null)
            {
                draw_Node(subNode,i);

                if (subNode.leftChild != null)
                {
                    subNode.leftChild.x = subNode.x - 25 * (int)Math.Pow(2, i - 1);
                    subNode.leftChild.y = subNode.y + 100;
                }
                if (subNode.rightChild != null)
                {

                    subNode.rightChild.x = subNode.x + 25 * (int)Math.Pow(2, i - 1);
                    subNode.rightChild.y = subNode.y + 100;
                }
                i--;
                pre_Order(subNode.leftChild, i);
                pre_Order(subNode.rightChild, i);

            }
        }
        private void draw_Node(TreeNode subNode, int i)
        {
            //画节点

            Canvas canvNode = new Canvas();
            canvNode.Height = 50;
            canvNode.Width = 50;
            SolidColorBrush brush = new SolidColorBrush();

            //层颜色
            switch (i)
            {
                case 0:
                    brush.Color = Color.FromRgb(173, 255, 47);
                    break;
                case 1:
                    brush.Color = Color.FromRgb(255, 181, 161);
                    break;
                case 2:
                    brush.Color = Color.FromRgb(95, 217, 205);
                    break;
                case 3:
                    brush.Color = Color.FromRgb(234, 247, 134);
                    break;
                case 4:
                    brush.Color = Color.FromRgb(184, 255, 184);
                    break;
                default:
                    brush.Color = Color.FromRgb(255, 255, 255);
                    break;

            }
            canvNode.Children.Add(new Ellipse
            {

                //Stroke = Brushes.GreenYellow,
                //StrokeThickness = 2,
                Height = 50,
                Width = 50
            });

            //Label labelData = new Label();

            //labelData.Content = subNode.data;
            //labelData.FontFamily = new FontFamily("iconfont");
            //labelData.VerticalContentAlignment = VerticalAlignment.Center;
            //labelData.HorizontalContentAlignment = HorizontalAlignment.Center;
            //labelData.Height = 50;
            //labelData.Width = 50;
            //labelData.FontSize = 26;
            TextBlock textData = new TextBlock();
            //textData.Text = subNode.data.ToString();
            textData.Foreground = brush;
            textData.TextAlignment = TextAlignment.Center;
            textData.HorizontalAlignment = HorizontalAlignment.Center;
            textData.VerticalAlignment = VerticalAlignment.Center;
            textData.Height = 50;
            textData.Width = 50;
            textData.FontSize = 45;
            textData.Margin = new Thickness(0, 0, 0, 3);
            if (subNode.data != 0)
            {
                if(subNode.data==' ')
                {
                    textData.Text = "' '";
                    textData.FontFamily = labelFamily.FontFamily;
                }
                else if (subNode.data == '\r')
                {
                    textData.Text = "\\r";
                    textData.FontFamily = labelFamily.FontFamily;
                }
                else if (subNode.data == '\n')
                {
                    textData.Text = "\\n";
                    textData.FontFamily = labelFamily.FontFamily;

                }
                else if (subNode.data == 'x')
                {
                    textData.Text = "x";
                    textData.FontSize = 45;
                    textData.Margin = new Thickness(0, -5, 0, 3);
                }
                else 
                {
                    textData.Text = subNode.data.ToString();
                    textData.FontFamily = labelFamily.FontFamily;
                }
                
            }
            else
            {
                textData.Text = "\xe63a";
                textData.FontFamily = labelFamily.FontFamily;
            }
            
            canvNode.Children.Add(textData);

            Canvas.SetLeft(canvNode, subNode.x);
            Canvas.SetTop(canvNode, subNode.y);
            canvNode.Visibility = Visibility.Hidden;
          
            canvasDic.Add(subNode.id, canvNode);

            canvas1.Children.Add(canvNode);
            
        }
        //画左线
        private void draw_LeftLine(TreeNode subNode)
        {
            Line line = new Line();
            line.X1 = subNode.x + 25;
            line.Y1 = subNode.y + 50;
            line.X2 = subNode.leftChild.x + 25;
            line.Y2 = subNode.leftChild.y;
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color= Color.FromRgb(219, 234, 144);
            line.Stroke = brush;
            line.StrokeThickness = 5;
           
            canvas1.Children.Add(line);

            Label label = new Label();
            label.Content = 0;
            label.Height = 35;
            label.Width = 35;
            label.FontSize = 16;
            Canvas.SetLeft(label, (line.X1 + line.X2) / 2 - 20);
            Canvas.SetTop(label, (line.Y1 + line.Y2) / 2 - 20);

            
             canvas1.Children.Add(label);
            
        }
        //画右线
        private void draw_RightLine(TreeNode subNode)
        {
            Line line = new Line();
            line.X1 = subNode.x + 25;
            line.Y1 = subNode.y + 50;
            line.X2 = subNode.rightChild.x + 25;
            line.Y2 = subNode.rightChild.y;
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Color.FromRgb(219, 234, 144);
            line.Stroke = brush;
            line.StrokeThickness = 5;
            
            canvas1.Children.Add(line);
            
            Label label = new Label();
            label.Content = 1;
            label.Height = 35;
            label.Width = 35;
            label.FontSize = 16;
            Canvas.SetLeft(label, (line.X1 + line.X2) / 2);
            Canvas.SetTop(label, (line.Y1 + line.Y2) / 2 - 20);
           
            canvas1.Children.Add(label);
            
        }
        private void init_Nodes()
        {
            nodes = new List<TreeNode>();
            foreach (char key in tempDic.Keys)
            {
                nodes.Add(new TreeNode(key, tempDic[key]));
            }
        }
        private int get_High(TreeNode subNode)
        {
            if (subNode != null)
            {
                return get_High(subNode.leftChild) > get_High(subNode.rightChild) ?
                    get_High(subNode.leftChild) + 1 : get_High(subNode.rightChild) + 1;
            }
            return 0;
        }
        //清空数据
        public void reset()
        {
            tempDic.Clear();
            nodes.Clear();
            canvasDic.Clear();
            nodeDic.Clear();
        }
    }
}
