#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <kunde.h>
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




