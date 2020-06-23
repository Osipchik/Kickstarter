import React, { Component } from 'react';
import { Route, Switch, BrowserRouter as Router} from "react-router-dom";
import Home from './components/Pages/Home';
import { Company } from "./components/Pages/PreviewPage/Company";
import { Explore } from "./components/Pages/ExplorePage/Explore";
import { CreateCompany } from "./components/Pages/StartAProject/CreateCompany";
import { Preview }  from './components/Pages/PreviewPage/Preview';
import { NotFound } from "./components/Pages/NotFound";
import NavMenu from "./components/Decorators/NavMenu/NavMenu";
import CssBaseline from "@material-ui/core/CssBaseline";
import Fab from "@material-ui/core/Fab";
import KeyboardArrowUpIcon from "@material-ui/icons/KeyboardArrowUp";
import { ScrollTop } from "./components/Decorators/NavMenu/ScrollEffect";
import { PrivateRoute } from "./PrivateRoute";
import { AppRoutes } from "./Helpers/AppRoutes";
import 'antd/dist/antd.css';
import './styles/Colors.css';
import './styles/custom.css';
import './styles/ScrollBar.css';

import { connect } from "react-redux";
import CallbackPage from './auth/CallBack';

class App extends Component {

    render () {
        return (
            <Router>
                <div>
                    <CssBaseline />
                    <NavMenu />

                    <Switch >
                        <Route path="/auth-callback" component={CallbackPage} />

                        <Route exact path={AppRoutes.Home} component={Home} />
                        <Route path={AppRoutes.Company} component={Company}/>
                        <Route path={AppRoutes.Explore} component={Explore}/>                        

                        <PrivateRoute exact user={this.props.user} path={AppRoutes.CreateCompany} component={CreateCompany}/>
                        <PrivateRoute exact user={this.props.user} path={AppRoutes.Preview} component={Preview}/>
                            
                        <Route exact component={NotFound}/>
                    </Switch>

                    <ScrollTop {...this.props}>
                        <Fab color="primary" size="medium" aria-label="scroll back to top">
                            <KeyboardArrowUpIcon />
                        </Fab>
                    </ScrollTop>
                </div>
            </Router>
        );
    }
}

function mapStateToProps(state) {
    return {
        user: state.oidc.user,
    };
  }

  export default connect(mapStateToProps)(App);