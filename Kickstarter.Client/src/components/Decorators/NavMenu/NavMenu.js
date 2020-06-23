import React from 'react';
import {MainNav} from "./MainNav";
import NavTabs from "./CompanyCreationNav";
import { withRouter } from "react-router-dom"


const NavMenu = (props) => {

    return (
        <header id="back-to-top-anchor">
            {props.location.pathname.includes('/CreateCompany') 
                ? <NavTabs history={props.history}/> 
                : <MainNav history={props.history}/>}
        </header>
    );
}

export default withRouter(NavMenu);