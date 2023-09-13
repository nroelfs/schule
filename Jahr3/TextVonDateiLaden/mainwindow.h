#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <string>
QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

private slots:

//    void on_btnExit_clicked();

//    void on_btnSave_clicked();

//    void on_btnNew_clicked();

//    void on_btnLoad_clicked();


    void on_action_Neu_triggered();

    void on_action_Speichern_triggered();

    void on_action_Laden_triggered();

    void on_action_Programm_Beenden_triggered();

private:
    Ui::MainWindow *ui;
};
#endif // MAINWINDOW_H
