using System;
using System.Windows.Input;

namespace SAOResoForm.Common
{
    /// <summary>
    /// Implementazione generica di ICommand per pattern MVVM
    /// Esempio: RelayCommand<Personale> per passare oggetti Personale come parametro
    /// </summary>
    /// <typeparam name="T">Tipo del parametro del comando (es. Personale, int, string)</typeparam>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        /// <summary>
        /// Crea un nuovo RelayCommand
        /// </summary>
        /// <param name="execute">Azione da eseguire quando il comando viene invocato</param>
        /// <param name="canExecute">Funzione che determina se il comando può essere eseguito (opzionale)</param>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Determina se il comando può essere eseguito
        /// </summary>
        public bool CanExecute(object parameter)
        {
            // Se non c'è canExecute, il comando è sempre eseguibile
            if (_canExecute == null)
                return true;

            // Gestione parametro null
            if (parameter == null)
            {
                // Se T è nullable o reference type, passa default(T)
                if (!typeof(T).IsValueType || Nullable.GetUnderlyingType(typeof(T)) != null)
                {
                    return _canExecute(default(T));
                }
                // Se T è value type non nullable e parameter è null, non può essere eseguito
                return false;
            }

            // Verifica tipo e esegue canExecute
            return parameter is T typedParameter && _canExecute(typedParameter);
        }

        /// <summary>
        /// Esegue il comando
        /// </summary>
        public void Execute(object parameter)
        {
            // Gestione parametro null
            if (parameter == null)
            {
                // Se T è nullable o reference type, esegui con default(T)
                if (!typeof(T).IsValueType || Nullable.GetUnderlyingType(typeof(T)) != null)
                {
                    _execute(default(T));
                    return;
                }
                // Se T è value type non nullable, non fare nulla
                return;
            }

            // Esegue solo se il tipo è corretto
            if (parameter is T typedParameter)
            {
                _execute(typedParameter);
            }
        }

        /// <summary>
        /// Evento che notifica quando CanExecute potrebbe essere cambiato
        /// Si hook automaticamente al CommandManager.RequerySuggested
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Forza la riesecuzione di CanExecute su tutti i comandi
        /// Utile quando lo stato dell'applicazione cambia manualmente
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    /// <summary>
    /// RelayCommand senza parametro per retrocompatibilità
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}