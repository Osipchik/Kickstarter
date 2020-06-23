﻿import React, { Component } from 'react';
import { CompaniesCard } from "../Decorators/companyCards/CompaniesCard";
import { Loading } from "../Decorators/Loading";
import { ApiFetch } from '../../Helpers/ApiCall';
import { PreviewEndpoints } from '../../Helpers/AppRoutes';
import { withTranslation } from 'react-i18next'


class Home extends Component {

    take = 10;
    
    constructor(props) {
        super(props);

        this.t = props.t

        this.state = {
            companies: [],
            loading: true,

            selectedDate: new Date(),
        };
    }

    async componentDidMount(){
        
        let params = new URLSearchParams({
            take: this.take,
            skip: this.state.companies.length,
        });

        let newCompanies = await ApiFetch(PreviewEndpoints.GetPreviews + params.toString())

        if (newCompanies !== undefined) {
            newCompanies = this.state.companies.concat(newCompanies);
            
            console.log(newCompanies)

            this.setState({
                companies: newCompanies,
                loading: false,
            });
        }
    }

    render() {
        return (
            <section>            
                {
                    this.state.loading
                        ? <Loading color="secondary"/>
                        : <CompaniesCard companies={this.state.companies}/>
                }
            </section>
        );
    }
}

export default withTranslation()(Home)