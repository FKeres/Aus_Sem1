internal class Program
{
    private static void Main(string[] args)
    {

        all();
        //test(755);
       
    }

    public static void all() {
        int i = 1;
        while(true) {
            Test test = new Test(100000, 2, i);
            bool answer = test.TestOperations();
            Console.WriteLine(i);
            if(!answer) {
                throw new ArgumentException(i.ToString());
            }
            i++;
        }
    }

    public static void test(int num) {
         
        Test test = new Test(100000, 2, num);
        bool answer = test.TestOperations();
        Console.WriteLine(answer); 
        
    }


}