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
    Kunde kunde("","","");
    this->setTextLabelsAndNavigationButtons(kunde);
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
    Kunde NeuerKunde(
        ui->lineEditNachname->text(),
        ui->lineEditVorname->text(),
        ui->lineEditTelefonnummer->text());

    Kunde emptyKunde("","","");
    if(NeuerKunde != emptyKunde){
        int index = Kunden.indexOf(NeuerKunde);
        if(index == -1){
            Kunden.push_back(NeuerKunde);
        }else{
            //Kunden[index] = NeuerKunde;
            QMessageBox::information(this, "Kunde Bereits vorhanden","Der Kunde ist bereits vorhanden!");
        }
        this->setTextLabelsAndNavigationButtons(NeuerKunde);
    }else{
        QMessageBox::warning(this, "Fehler!","Der Kunde darf nicht leer sein!");
    }

}

void MainWindow::on_action_Speichern_triggered()
{
    QString KundenToSave = this->ToJson(Kunden);
    QString saveFileName = QFileDialog().getSaveFileName();

    if(saveFileName == "" || saveFileName == NULL){
        saveFileName = default_file;
    }
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
void MainWindow::on_action_Laden_triggered()
{
    QString loadFileName = QFileDialog().getOpenFileName();

    if(loadFileName == "" || loadFileName == NULL){
        loadFileName = default_file;
    }
    QFile file(loadFileName);

    if(file.open(QIODevice::ReadOnly | QIODevice::Text))
    {
        Kunden.clear();
        QTextStream in(&file);
        QString kundenAsString = in.readAll();
        file.close();
        QJsonDocument doc = QJsonDocument::fromJson(kundenAsString.toUtf8());
        QJsonArray kundenAsJsonArray = doc.array();

        for(const QJsonValue &kunde : kundenAsJsonArray){
            if(kunde.isObject()){
                QJsonObject tmpKunde = kunde.toObject();
                Kunde kundeToObjet;
                kundeToObjet.fromQJsonObject(tmpKunde);
                Kunden.push_back(kundeToObjet);
            }
        }
        if( Kunden.size() > 0){
            if(loadFileName == default_file){
                QMessageBox::information(this, "Speichern erfolgreich","Kontakte wurden aus dem lokalen speicher geladen");
            }
            this->setTextLabelsAndNavigationButtons(Kunden.first());
        }else{
            QMessageBox::warning(this,"Fehler","Fehler beim lesen der Datei");
        }



    }
    else
    {
        QMessageBox::warning(this,"Fehler","Fehler beim Öffnen der Datei!");
    }
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

void MainWindow::setTextLabelsAndNavigationButtons(Kunde kunde){
    ui->lineEditNachname->setText(kunde.GetNachname());
    ui->lineEditVorname->setText(kunde.GetVorname());
    ui->lineEditTelefonnummer->setText(kunde.GetTel());
    QString maxCount =  QString::number(Kunden.size());
    int index = Kunden.indexOf(kunde);
    if (index != -1) {
        QString IndexAsString = QString::number(index +1);

        ui->labelAnzahl->setText("Eintrag " + IndexAsString  + " von " + maxCount);

        ui->pushButtonZurueck->setEnabled(index > 0);
        ui->pushButtonWeiter->setEnabled(index < Kunden.size() - 1);
    } else {
        ui->labelAnzahl->setText("Eintrag 0 von " + maxCount);
        ui->pushButtonZurueck->setEnabled(0);
        ui->pushButtonWeiter->setEnabled(0);
    }
}




void MainWindow::on_pushButtonZurueck_clicked()
{
       Kunde tmp(
                ui->lineEditNachname->text(),
                ui->lineEditVorname->text(),
                ui->lineEditTelefonnummer->text());
    int currentIndex = Kunden.indexOf(tmp);
    if(currentIndex > -1){
        this->setTextLabelsAndNavigationButtons(Kunden[currentIndex - 1]);
    }else{
       QMessageBox::StandardButton move;
       move = QMessageBox::question(this, "Zurueck", "Wenn Sie mit dieser Action fortfahren wird ihr neuer Eintrag entfernt werden",
                                    QMessageBox::Yes|QMessageBox::No);
       if(move == QMessageBox::Yes){
            this->setTextLabelsAndNavigationButtons(Kunden[0]);
       }
    }
}


void MainWindow::on_pushButtonWeiter_clicked()
{

    Kunde tmp(
        ui->lineEditNachname->text(),
        ui->lineEditVorname->text(),
        ui->lineEditTelefonnummer->text());
    int currentIndex = Kunden.indexOf(tmp);
    if(currentIndex > -1){
        this->setTextLabelsAndNavigationButtons(Kunden[currentIndex + 1]);
    }else{
       QMessageBox::StandardButton move;
       move = QMessageBox::question(this, "Weiter", "Wenn Sie mit dieser Action fortfahren wird ihr neuer Eintrag entfernt werden",
                                    QMessageBox::Yes|QMessageBox::No);
       if(move == QMessageBox::Yes){
            this->setTextLabelsAndNavigationButtons(Kunden[0]);
       }
    }
}


void MainWindow::on_action_Beenden_triggered()
{
    QMessageBox::information(this, "Programm wird beendet","Alle ungespeicherten Änderungen werden gelöscht");
    QApplication::quit();
}

