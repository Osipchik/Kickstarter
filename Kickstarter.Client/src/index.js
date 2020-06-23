import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import { Provider } from 'react-redux'
import { ThemeProvider as MuiThemeProvider } from '@material-ui/core/styles';
import { theme } from "./Theme";
import { SnackbarProvider } from 'notistack';
import { OidcProvider } from 'redux-oidc';
import userManager from './auth/userManager';

import store from './modules/store';

const rootElement = document.getElementById('root');

ReactDOM.render(
    <Provider store={store}>
        <OidcProvider store={store} userManager={userManager}>
            <MuiThemeProvider theme={theme}>
                <SnackbarProvider anchorOrigin={{vertical: 'top',horizontal: 'center'}}>
                    <App />
                </SnackbarProvider>
            </MuiThemeProvider>
        </OidcProvider>
    </Provider>,
    rootElement
);
