#ifndef MAINWINDOW_H
#define MAINWINDOW_H
#include <kunde.h>
#include <QMainWindow>

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
    void on_action_Neu_triggered();

    void on_pushButtonAdd_clicked();
    QString ToJson(QVector<Kunde>);

    void on_action_Speichern_triggered();

private:
    Ui::MainWindow *ui;
    QVector<Kunde> Kunden;
    QString default_file ="savedContacts.json";
};
#endif // MAINWINDOW_H
