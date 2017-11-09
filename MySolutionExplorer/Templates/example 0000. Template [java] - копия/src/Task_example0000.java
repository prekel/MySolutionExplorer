import java.io.*;
import java.util.*;

class Task {
    private static Scanner In;
    private static PrintWriter Out;

    public static void main(String[] args) throws Exception {
        if (args.length > 0) {
            In = new Scanner(new File("input.txt"));
            Out = new PrintWriter(new File("output.txt"));
        }
        else {
            In = new Scanner(System.in);
            Out = new PrintWriter(System.out);
        }

        String s[] = In.nextLine().split(" ");
        int a = Integer.parseInt(s[0]);
        int b = Integer.parseInt(s[1]);
        Out.println(a + b);

        Out.close();
    }
}
