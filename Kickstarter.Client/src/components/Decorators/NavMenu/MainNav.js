import React from 'react';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import IconButton from '@material-ui/core/IconButton';
import MenuIcon from '@material-ui/icons/Menu';
import Drawer from '@material-ui/core/Drawer';
import Logo from "../../../assets/kickstarter-logo-green.webp";
import { NavbarBrand, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import { NavUser } from "./NavUser/NavUser";
import { CreateCompany } from '../../../Helpers/ApiCall';
import '../../../styles/NavMenu.css';
import '../../../styles/LinkStyle.css';


export const MainNav = (props) => {
    const history = props.history
    const [state, setState] = React.useState({ open: false });

    const toggleDrawer = (open) => event => {
        setState({ ...state, open: open });
    };
    
    const onStartProject = async () => {
        let result = await CreateCompany();
        if (result !== null){
            history.push('/CreateCompany/' + result.id)
        }
    }

    const sideList = () => (
        <div
            className="side-list"
            role="presentation"
            onClick={toggleDrawer(false)}
            onKeyDown={toggleDrawer(false)}>
            <List>
                <ListItem button key={1}>
                    <NavUser/>
                </ListItem>
                <hr/>
                <ListItem button key={2}>
                    <Link className="link" to='/Explore/'>Explore</Link>
                </ListItem>
                <ListItem button key={3}>
                    <Link className="link" to="/CreateCompany">Start a project</Link>
                </ListItem>
                <ListItem button key={4}>
                    <Link className="link" to='/'>Search</Link>
                </ListItem>
            </List>
        </div>
      );

    return(
        <div className="flex-grow-1">
            <AppBar position="static" color="secondary">
                <Toolbar variant="dense" className="d-flex justify-content-between">
                    <IconButton 
                        edge="start" 
                        className="d-md-none menu-button"
                        onClick={toggleDrawer(true)}
                        aria-label="menu">
                        <MenuIcon /> 
                    </IconButton>
                    <Drawer open={state.open} onClose={toggleDrawer(false)}>
                        {sideList()}
                    </Drawer>    
                    <div className="row col d-none d-md-inline-flex">
                        <NavLink className="link" tag={Link} to='/Explore/'>Explore</NavLink>
                        <NavLink className="link" onClick={onStartProject}>Start a project</NavLink>
                    </div>
                    <NavbarBrand tag={Link} to="/" className="mb-2 mx-auto">
                        <img src={Logo} alt="logo" height={15} />
                    </NavbarBrand>
                    <div className="row col justify-content-end d-none d-md-inline-flex">
                        <NavLink className="link" tag={Link} to='/'>Search</NavLink>
                        <NavUser/>
                    </div>
                </Toolbar>
            </AppBar>
        </div>
    );
}