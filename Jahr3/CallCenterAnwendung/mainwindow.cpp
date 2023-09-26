#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <kunde.h>
#include <QVector>
#include <QJsonArray>
#include <QJsonDocument>
#include <QFileDialog>
#include <QMessageBox>
#include <QTextStream>
#include <QFile>

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


void MainWindow::on_action_Neu_triggered()
{
    ui->lineEditNachname->clear();
    ui->lineEditVorname->clear();
    ui->lineEditTelefonnummer->clear();
}





void MainWindow::on_pushButtonAdd_clicked()
{
    QString nName = ui->lineEditNachname->text();
    QString vName = ui->lineEditVorname->text();
    QString tel = ui->lineEditTelefonnummer->text();
    Kunde NeuerKunde(vName,nName,tel);
    Kunden.push_back(NeuerKunde);
}

QString MainWindow::ToJson(QVector<Kunde>Kunden){
    QJsonArray KundenArray;
    for (Kunde kunde : Kunden) {
        KundenArray.append(kunde.toQJsonObject());
    }
    QJsonObject rootKundenarray;
    rootKundenarray["Kunden"] = KundenArray;
    QJsonDocument doc;
    doc.setArray(KundenArray);
    QString JSON(doc.toJson());
    return JSON;
}



void MainWindow::on_action_Speichern_triggered()
{
    QString KundenToSave = this->ToJson(Kunden);
    QString saveFileName = default_file;
    saveFileName = QFileDialog().getSaveFileName();


    QFile file(saveFileName);

    if(file.open(QIODevice::WriteOnly | QIODevice::Text))
    {
        QTextStream out(&file);
        out << KundenToSave;
        file.close();

    }
    else
    {
        QMessageBox::warning(this,"Fehler","Fehler beim Speichern!");
    }
    if(saveFileName == default_file){
        QMessageBox::information(this, "Speichern erfolgreich","Kontakte wurden lokal gespeichert");
    }else{
        QMessageBox::information(this, "Speichern erfolgreich","Kontakte wurden in Der Datei: " + saveFileName + " gespeichert");
    }


}

