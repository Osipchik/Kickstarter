import React, {Component} from 'react';
import CompaniesCard from "../../Decorators/companyCards/CompaniesCard";
import Button from "@material-ui/core/Button";
import {Loading} from "../../Decorators/Loading";
import Divider from "@material-ui/core/Divider";
import {CustomButton} from "../../Decorators/Buttons";
import Categories from "./Categories";
import Sort from "./Sort";
import { Input } from 'antd';
const { Search } = Input;


export class Explore extends Component {
    take = 30;

    constructor(props){
        super(props);

        this.state = {
            currentCategoryId: props.match.params.categoryId,
            currentSubCategoryId: props.match.params.subcategoryId,
            currentCategoryTitle: 'All',
            companies: [],
            loading: true,
            history: this.props.history,
            setAnchorEl: null,
            anchorEl: null,
        };
        
        this.loadMore = this.loadMore.bind(this);
        this.onSearch = this.onSearch.bind(this);
        this.onCategoryClick = this.onCategoryClick.bind(this);
    }

    async componentDidMount(){
        this.setState({
            // companies: await ApiRequest.GetCompanyPreviewAsync(this.take, this.state.companies, this.state.currentCategoryId, this.state.currentSubCategoryId),
            loading: false,
        });
    }

    async loadMore(){
        if(!this.state.loading){
            this.setState({
                loading: true
            });

            this.setState({
                // companies: await ApiRequest.GetCompanyPreviewAsync(this.take, this.state.companies, this.state.currentCategoryId, this.state.currentSubCategoryId),
                loading: false,
            });
        }
    }

    async onCategoryClick(condition){
        if(this.state.loading){
            return;
        }
        if(this.state.currentCategoryId === condition.categoryId && this.state.currentSubCategoryId === condition.subCategoryId){
            return
        }

        this.setState({
            loading: true,
            companies: []
        });

        this.setState({
            // companies: await ApiRequest.GetCompanyPreviewAsync(this.take, [], condition.categoryId, condition.subCategoryId),
        });
        
        this.setState({
            loading: false,
            currentCategoryId: condition.categoryId,
            currentCategoryTitle: condition.title,
            currentSubCategoryId: condition.subCategoryId,
        });
    }

    onSearch(event) {
        console.log('search');
        console.log(event.target.value);
    }

    onSelect(value) {
        console.log(`selected ${value}`);
    }

    render() {
        let condition = this.state.loading
            ? <Loading color="secondary"/>
            : <CustomButton onClick={this.loadMore} variant="contained">Load more</CustomButton>;
        
        return(
            <section >
                <div className="py-3 w-100">
                    <div className="row">
                        <Categories
                            handelClick={this.onCategoryClick}
                            defaultSelectedSubKey={this.state.currentSubCategoryId}
                            expandedKey={this.state.currentCategoryId}/>
                        <div className="col-sm-12 col-md-10 col-lg-9 col-xl-10 m-0 p-0">
                            <div className="mx-4">
                                <div>
                                    <Search placeholder="Search" onChange={this.onSearch}/>
                                    <Divider className="my-3"/>
                                </div>
                                <div className="row pb-1">
                                    <div className="col">
                                        <Button className="d-md-none">
                                            {this.state.currentCategoryTitle}
                                        </Button>
                                    </div>
                                    <Sort onSelect={this.onSelect}/>
                                </div>
                            </div>
                            <CompaniesCard companies={this.state.companies}/>
                            <div className="text-center">
                                {condition}
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        )
    }
}