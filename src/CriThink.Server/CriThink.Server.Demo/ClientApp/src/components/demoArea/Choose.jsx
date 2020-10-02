import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { WaveDown } from './../Layout';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { getDemoNews, selectNews } from '../../actions/demo';
import {
    Dropdown,
    Segment,
    Icon,
    Button,
    Popup,
    Loader,
    Dimmer,
} from 'semantic-ui-react';

class ChooseArea extends Component {
    componentDidMount () {
        if (this.props.demoLinks.length === 0) {
            this.props.getDemoNews();
        }
    }

    handleSelection = (event, data) => {
        const news = data.options.find((o) => o.value === data.value);
        this.props.selectNews({
            uri: news.value,
            title: news.text,
        });
    };

    render () {
        return (
            <div>
                <WaveDown namePage="Check Your News">
                    <Segment basic size="massive">
                        <span className="light">CHECK THE NEWS</span>
                        <Popup
                            content="Pick a news from the list"
                            trigger={
                                <Icon
                                    name="info circle"
                                    style={{ color: '#FF9600', margin: '1rem' }}
                                />
                            }
                        />
                        <Segment basic>
                            <Dropdown
                                className="light dropdowncss"
                                fluid
                                selection
                                options={this.props.demoLinks}
                                onChange={this.handleSelection}
                            />
                            <Segment basic>
                                <Link to="/analysis">
                                    <Button className="btnorange">
                                        <span className="regular">SEE THE RESULT</span>
                                    </Button>
                                </Link>
                            </Segment>
                            <Dimmer active={this.props.loading} inverted>
                                <Loader />
                            </Dimmer>
                        </Segment>
                    </Segment>
                </WaveDown>
            </div>
        );
    }
}

function mapStateToProps (state) {
    return {
        demoLinks: state.demo.demoNews.map(news => ({
            value: news.url,
            text: news.title,
        })),
        loading: !!state.app.loading.find(x => x.label === 'getDemoNews'),
    };
}

function mapDispatchToProps (dispatch) {
    return bindActionCreators({
        getDemoNews,
        selectNews,
    }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(ChooseArea);
