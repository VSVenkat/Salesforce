# Salesforce

#Apex

Apex is a strongly typed object oriented programming language

1. It allows developers to execute flows and transactional control statements
2. Apex language is
  1. Integrated: It provides built in support for DML calls.
  2. Inline salesforce object query language - SOQL
  3. Easy to use.
  4. Esay to test
  5. Version
  6. Multitenant architecture
  
  #Object Oriented Programming
  
  The main features of Object Oriented Programming are
  
  1. Encapsulation
  2. Inheritance
  3. Polymorphism
  
  Encapsulation : The wrapping up of data members and methods together is called as Encapsulation. For example, if we take a class we put data and methods inside the class, so the class is binding them together in a single class. So class is an example of Encapsulation.
  
  Inheritance : It allows to inherit the features from one class to other class . The class which aquires the features are called derived class and class which provides the features to the derived class is called Base class/Super class. The process to inheriting the features(data members and data methods from Parent class to child class is called Inheritance.
  
  Polymorphism : Representing one form in multiple forms is called polymorphism.
  
  #Class :
  
  Class is a collection of data members and methods.
  
 Eg :  class Student{
    Integer no;
    String name;
    
    public void getDetails(){
      System.debug("Roll no. : "+no);
      System.debug("Student name :"+name);
    }
  
  }
  
  Eg 2: 
  class Employee{
    Integer exp;
    String department;
    
    public void show(){
      logic...
      .....
            
    }
  }
  
  
  
  
  
  
  
  
  
