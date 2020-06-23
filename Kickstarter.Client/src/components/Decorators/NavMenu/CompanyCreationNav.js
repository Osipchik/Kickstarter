import React from 'react';
import AppBar from '@material-ui/core/AppBar';
import {NavbarBrand} from "reactstrap";
import {Link} from "react-router-dom";
import Logo from "../../../assets/kickstarter-logo-green.webp";
import {Toolbar} from "@material-ui/core";
import {WhiteButton} from "../Buttons";
import VisibilityIcon from '@material-ui/icons/Visibility';
import { withRouter } from 'react-router-dom';

const NavTabs = (props) => {
    const goBack = () => {
        props.history.goBack();
    };

    const goToPreview = () => {
        let path = '/Preview';
        props.history.push(path);
    };

    return(
        <div className="flex-grow-1">
            <AppBar position="static" color="secondary" elevation={0} className="border-bottom">
                <Toolbar variant="dense">
                    <NavbarBrand tag={Link} to="/" className="mr-auto mb-2" onClick={() => goBack}>
                        <img src={Logo} alt="logo" height={15} />
                    </NavbarBrand>
                    <WhiteButton
                        variant="contained"
                        color="secondary"
                        onClick={() => goToPreview()}
                        startIcon={<VisibilityIcon />}>
                        Preview
                    </WhiteButton>
                </Toolbar>
            </AppBar>
        </div>
    );
}

export default withRouter(NavTabs)