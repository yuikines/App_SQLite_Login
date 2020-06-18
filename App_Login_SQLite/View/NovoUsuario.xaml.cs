using App_Login_SQLite.Model;
using App_Login_SQLite.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App_Login_SQLite.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NovoUsuario : ContentPage
    {
        int id;
        public NovoUsuario()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            if (Menu.usuario != null)
            {
                btCadastrar.Text = "Alterar";
                btExcluir.IsVisible = true;
                btCancelar.IsVisible = false;
                lblTitulo.Text = "ALTERAR SENHA";
                //carrega os componentes com os valores passados através do construtor
                id = Menu.usuario.Id;
                TxtUsuario.Text = Menu.usuario.Nome;
                txtSenha.Text = Menu.usuario.Senha;
                NavigationPage.SetHasNavigationBar(this, true);
            }
        }

        private async void Button_Clicked_Novo(object sender, EventArgs e)
        {
            try
            {
                ModelUsuario user = new ModelUsuario();
                user.Nome = TxtUsuario.Text;
                user.Senha = txtSenha.Text;
                ServiceBDUsuario dbAula = new ServiceBDUsuario(App.DbPath);
                if (btCadastrar.Text == "CRIAR NOVO USUÁRIO")
                {
                    dbAula.Inserir(user);
                    await DisplayAlert("Resultado da operação", dbAula.StatusMessage, "OK");
                    await Navigation.PopAsync();
                }
                else
                { //alterar, em proxima aula
                    user.Id = id;
                    dbAula.Alterar(user);
                    await DisplayAlert("Resultado da operação", dbAula.StatusMessage, "OK");
                    Menu.usuario.Nome = TxtUsuario.Text;
                    Menu.usuario.Senha = txtSenha.Text;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void BtCancelar_Clicked(object sender, EventArgs e)
        {
            if (btCadastrar.Text == "CRIAR NOVO USUÁRIO")
            {
                await Navigation.PushAsync(new Login());
            }
        }

        private async void BtExcluir_Clicked(object sender, EventArgs e)
        {
            //pergunta para confimar a exclusão do registro
            bool resp = await DisplayAlert("Excluir Registro",
                                            "Deseja excluir o usuário atual ?",
                                            "Sim", "Não");
            //se o retorno true, o método excluir é chamado
            if (resp)
            {
                ServiceBDUsuario dbAula = new ServiceBDUsuario(App.DbPath);
                dbAula.Excluir(id);
                await DisplayAlert("Resultado da operação", dbAula.StatusMessage, "OK");
                Menu.usuario = null;
                await Navigation.PushAsync(new Login());
            }
        }
    }
}