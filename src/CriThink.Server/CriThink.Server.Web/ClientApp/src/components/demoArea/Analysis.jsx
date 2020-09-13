/* eslint-disable no-mixed-spaces-and-tabs */
import React, { Component } from 'react';
import { Redirect } from 'react-router-dom';
import { WaveDown } from './../Layout';
import { Menu, Segment, Loader, Dimmer } from 'semantic-ui-react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { getQuestions, getNews, getNewsClassification } from '../../actions/demo';
import MenuRender from './MenuRender';
import AnalysisNews from './AnalysisNews';
import LogoAnim from './LogoAnim';

class AnalysisArea extends Component {
    constructor (props) {
        super(props);

        this.state = {
            activeItem: 'SOURCE ANALYSIS',
        };
    }

    componentDidMount () {
        const { newsLink } = this.props;
        this.props.getQuestions();
        this.props.getNews(newsLink);
        this.props.getNewsClassification(newsLink);
    }

	handleItemClick = (e, { name }) => this.setState({ activeItem: name });

	switchRender = () => {
	    if (this.props.loading) {
	        return(<Dimmer active inverted>
	            <Loader />
	        </Dimmer>);
	    }

	    switch (this.state.activeItem) {
	        case 'SOURCE ANALYSIS':
	            return (
	                <MenuRender
	                    color={this.props.color}
	                    header={`Domain is ${ this.props.newsClassification }`}
	                    uppercase
	                >
	                    <Segment basic>{this.props.classificationDescription}</Segment>
	                </MenuRender>
	            );
	        case 'KEY WORDS':
	            return (
	                <MenuRender
	                    color="grey"
	                    header="Section under construction..."
	                ></MenuRender>
	            );
	        case 'ANALYSIS':
	            return (
	                <MenuRender header={this.props.newsHeader}>
	                    <AnalysisNews body={this.props.newsBody}>
	                        <LogoAnim msgPopup={this.props.head} letter="H" />
	                        <LogoAnim
	                            msgPopup={this.props.evidence}
	                            letter="E"
	                        />
	                        <LogoAnim
	                            msgPopup={this.props.accurancy}
	                            letter="A"
	                        />
	                        <LogoAnim
	                            msgPopup={this.props.deceiving}
	                            letter="D"
	                        />
	                    </AnalysisNews>
	                </MenuRender>
	            );
	        case 'CHECK ANOTHER NEWS':
	            return <Redirect to="/3" />;
	        default:
	            return null;
	    }
	}

	render () {
	    const { activeItem } = this.state;
	    return (
	        <div>
	            {this.props.newsLink == null ? <Redirect to="/3" /> : null}
	            <WaveDown namePage="Check Your News">
	                <Menu pointing secondary size="huge">
	                    <Menu.Item
	                        name="SOURCE ANALYSIS"
	                        active={activeItem === 'SOURCE ANALYSIS'}
	                        onClick={this.handleItemClick}
	                    />
	                    <Menu.Item
	                        name="KEY WORDS"
	                        active={activeItem === 'KEY WORDS'}
	                        onClick={this.handleItemClick}
	                    />
	                    <Menu.Item
	                        name="ANALYSIS"
	                        active={activeItem === 'ANALYSIS'}
	                        onClick={this.handleItemClick}
	                    />
	                    <Menu.Menu position="right">
	                        <Menu.Item
	                            name="CHECK ANOTHER NEWS"
	                            active={activeItem === 'CHECK ANOTHER NEWS'}
	                            onClick={this.handleItemClick}
	                        />
	                    </Menu.Menu>
	                </Menu>
	                {this.switchRender()}
	            </WaveDown>
	        </div>
	    );
	}
}

function mapStateToProps (state) {
    return {
        head: state.demo.questionH,
        evidence: state.demo.questionE,
        accurancy: state.demo.questionA,
        deceiving: state.demo.questionD,
        newsHeader: state.demo.newsHeader,
        newsBody: state.demo.newsBody,
        newsLink: state.demo.demoNewsSelected.uri,
        newsClassification: state.demo.demoNewsSelected.classification,
        classificationDescription: state.demo.demoNewsSelected.description,
        color: state.demo.demoNewsSelected.color,
        loading: !!state.app.loading.find(x => x.label === 'getDemoNews')
			|| !!state.app.loading.find(x => x.label === 'getNewsClassification'),
    };
}

function mapDispatchToProps (dispatch) {
    return bindActionCreators({
        getQuestions,
        getNews,
        getNewsClassification,
    }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(AnalysisArea);
