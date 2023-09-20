#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QFile>
#include <QTextStream>
#include <QMessageBox>
MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{

    ui->setupUi(this);

}

MainWindow::~MainWindow()
{
    delete ui;
}




//void MainWindow::on_btnExit_clicked()
//{
//    QCoreApplication::quit();
//}
//void MainWindow::on_btnSave_clicked()
//{
//   QString name = ui->textEdit->toPlainText();
//   QString combinedText = "Hallo "+ name + ", schön dich kennenzulernen!";
//   ui->textEdit->setText(combinedText);
//   QFile file("vorherigeEingabe.txt");
//       if (file.open(QIODevice::WriteOnly | QIODevice::Text)) {
//           QTextStream out(&file);
//           out << name;
//           file.close();
//       }
//}
//void MainWindow::on_btnNew_clicked()
//{
//    ui->textEdit->setText("");
//}
//void MainWindow::on_btnLoad_clicked()
//{
//    QFile file("vorherigeEingabe.txt");
//    if (file.open(QIODevice::ReadOnly | QIODevice::Text)) {
//           QTextStream in(&file);
//           QString name = in.readAll();
//           QString combinedText = "Hallo " + name + ", schön dich kennenzulernen!";
//           ui->textEdit->setText(combinedText);
//           file.close();
//       }
//}


void MainWindow::on_action_Neu_triggered()
{
     ui->textEdit->setText("");
}


void MainWindow::on_action_Speichern_triggered()
{
    QString name = ui->textEdit->toPlainText();
    QString combinedText = "Hallo "+ name + ", schön dich kennenzulernen!";
    ui->textEdit->setText(combinedText);
    QFile file("vorherigeEingabe.txt");
        if (file.open(QIODevice::WriteOnly | QIODevice::Text)) {
            QTextStream out(&file);
            out << name;
            file.close();
        }
}


void MainWindow::on_action_Laden_triggered()
{
    QFile file("vorherigeEingabe.txt");
    if (file.open(QIODevice::ReadOnly | QIODevice::Text)) {
           QTextStream in(&file);
           QString name = in.readAll();
           QString combinedText = "Hallo " + name + ", schön dich kennenzulernen!";
           ui->textEdit->setText(combinedText);
           file.close();

       }

}


void MainWindow::on_action_Programm_Beenden_triggered()
{
      QCoreApplication::quit();
}


void MainWindow::on_pushButton_clicked()
{
    QString name = ui->textEdit->toPlainText();
    QString combinedText = "Hallo "+ name + ", schön dich kennenzulernen!";
    ui->textEdit->setText(combinedText);
    QFile file("vorherigeEingabe.txt");
        if (file.open(QIODevice::WriteOnly | QIODevice::Text)) {
            QTextStream out(&file);
            out << name;
            file.close();
        }
}

