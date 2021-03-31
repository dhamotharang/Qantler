import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';

/**
 * Elastic search API
 */
@Injectable({
  providedIn: 'root'
})
export class ElasticSearchService {
  /**
   *
   * @param http
   */
  constructor(private http: HttpClient) {
  }

  /**
   * get items from elastic by type
   *
   * @param _index
   * @param _type
   * @param _pageSize
   * @param _pageStart
   * @param sort
   * @param filter
   */
  getAllDocuments(_index, _type, _pageSize, _pageStart, sort = 'createdOn', filter: any = []): any {
    let sortData = {};
    if (sort === 'createdOn') {
      sortData = {'createdon': {'order': 'desc'}};
    } else if (sort === 'title') {
      sortData = {'title.keyword': {'order': 'asc'}};
    } else if (sort === 'rating') {
      sortData = {'rating': {'order': 'desc'}};
    } else {
      sortData = {'createdon': {'order': 'desc'}};
    }
    const filterData = [];
    if (filter.categories && filter.categories.length > 0) {
      filterData.push({
        'terms': {
          'categoryid': filter.categories
        }
      });
    }
    if (filter.subCategories && filter.subCategories.length > 0) {
      filterData.push({
        'terms': {
          'subcategoryid': filter.subCategories
        }
      });
    }
    if (filter.educationalStandard && filter.educationalStandard.length > 0) {
      filterData.push({
        'terms': {
          'educationalstandardid': filter.educationalStandard
        }
      });
    }
    if (filter.educationalUse && filter.educationalUse.length > 0) {
      filterData.push({
        'terms': {
          'educationaluseid': filter.educationalUse
        }
      });
    }
    if (filter.level && filter.level.length > 0) {
      filterData.push({
        'terms': {
          'levelid': filter.level
        }
      });
    }
    if (filter.materialType && filter.materialType.length > 0) {
      filterData.push({
        'terms': {
          'materialtypeid': filter.materialType
        }
      });
    }
    if (filter.copyright && filter.copyright.length > 0) {
      filterData.push({
        'terms': {
          'copyrightid': filter.copyright
        }
      });
    }
    if (filter.educations && filter.educations.length > 0) {
      filterData.push({
        'terms': {
          'educationid': filter.educations
        }
      });
    }
    if (filter.professions && filter.professions.length > 0) {
      filterData.push({
        'terms': {
          'professionid': filter.professions
        }
      });
    }
    return this.http.post(environment.apiUrl + 'Elastic', {
      partialURL: _index + '/' + _type + '/' + '_search',
      searchSTring: JSON.stringify({
        'query': {
          'bool': {
            'must': [
              {
                'match_all': {}
              },
              {'term': {'isapproved': true}}
            ],
            'filter': filterData,
            'must_not': []
          }
        },
        'sort': [sortData],
        'size': _pageSize,
        'from': _pageStart
      })
    });
  }

  /**
   * get all items from elastic
   *
   * @param _pageSize
   * @param _pageStart
   * @param sort
   * @param filter
   */
  advancedGetAllDocuments(_pageSize, _pageStart, sort = 'createdOn', filter: any = []): any {
    let sortData = {};
    if (sort === 'createdOn') {
      sortData = {'createdon': {'order': 'desc'}};
    } else if (sort === 'title') {
      sortData = {'title.keyword': {'order': 'asc'}};
    } else if (sort === 'rating') {
      sortData = {'rating': {'order': 'desc'}};
    } else {
      sortData = {'createdon': {'order': 'desc'}};
    }
    const filterData = [];
    if (filter.categories && filter.categories.length > 0) {
      filterData.push({
        'terms': {
          'categoryid': filter.categories
        }
      });
    }
    if (filter.subCategories && filter.subCategories.length > 0) {
      filterData.push({
        'terms': {
          'subcategoryid': filter.subCategories
        }
      });
    }
    if (filter.educationalStandard && filter.educationalStandard.length > 0) {
      filterData.push({
        'terms': {
          'educationalstandardid': filter.educationalStandard
        }
      });
    }
    if (filter.educationalUse && filter.educationalUse.length > 0) {
      filterData.push({
        'terms': {
          'educationaluseid': filter.educationalUse
        }
      });
    }
    if (filter.level && filter.level.length > 0) {
      filterData.push({
        'terms': {
          'levelid': filter.level
        }
      });
    }
    if (filter.materialType && filter.materialType.length > 0) {
      filterData.push({
        'terms': {
          'materialtypeid': filter.materialType
        }
      });
    }
    if (filter.copyright && filter.copyright.length > 0) {
      filterData.push({
        'terms': {
          'copyrightid': filter.copyright
        }
      });
    }
    if (filter.educations && filter.educations.length > 0) {
      filterData.push({
        'terms': {
          'educationid': filter.educations
        }
      });
    }
    if (filter.professions && filter.professions.length > 0) {
      filterData.push({
        'terms': {
          'professionid': filter.professions
        }
      });
    }

    return this.http.post(environment.apiUrl + 'Elastic', {
      partialURL: '_search',
      searchSTring: JSON.stringify({
        'query': {
          'bool': {
            'must': [
              {
                'match_all': {}
              },
              {'term': {'isapproved': true}}
            ],
            'filter': filterData,
            'must_not': []
          }
        },
        'sort': [sortData],
        'size': _pageSize,
        'from': _pageStart
      })
    });
  }

  /**
   * search item by type
   *
   * @param _index
   * @param _type
   * @param _pageSize
   * @param _pageStart
   * @param query
   * @param sort
   * @param filter
   */
  searchItem(_index, _type, _pageSize, _pageStart, query, sort = 'createdOn', filter: any = []): any {
    let sortData = {};
    if (sort === 'createdOn') {
      sortData = {'createdon': {'order': 'desc'}};
    } else if (sort === 'title') {
      sortData = {'title.keyword': {'order': 'asc'}};
    } else if (sort === 'rating') {
      sortData = {'rating': {'order': 'desc'}};
    } else {
      sortData = {'createdon': {'order': 'desc'}};
    }
    const filterData = [];
    if (filter.categories && filter.categories.length > 0) {
      filterData.push({
        'terms': {
          'categoryid': filter.categories
        }
      });
    }
    if (filter.subCategories && filter.subCategories.length > 0) {
      filterData.push({
        'terms': {
          'subcategoryid': filter.subCategories
        }
      });
    }
    if (filter.educationalStandard && filter.educationalStandard.length > 0) {
      filterData.push({
        'terms': {
          'educationalstandardid': filter.educationalStandard
        }
      });
    }
    if (filter.educationalUse && filter.educationalUse.length > 0) {
      filterData.push({
        'terms': {
          'educationaluseid': filter.educationalUse
        }
      });
    }
    if (filter.level && filter.level.length > 0) {
      filterData.push({
        'terms': {
          'levelid': filter.level
        }
      });
    }
    if (filter.materialType && filter.materialType.length > 0) {
      filterData.push({
        'terms': {
          'materialtypeid': filter.materialType
        }
      });
    }
    if (filter.copyright && filter.copyright.length > 0) {
      filterData.push({
        'terms': {
          'copyrightid': filter.copyright
        }
      });
    }
    if (filter.educations && filter.educations.length > 0) {
      filterData.push({
        'terms': {
          'educationid': filter.educations
        }
      });
    }
    if (filter.professions && filter.professions.length > 0) {
      filterData.push({
        'terms': {
          'professionid': filter.professions
        }
      });
    }

    return this.http.post(environment.apiUrl + 'Elastic', {
      partialURL: _index + '/' + _type + '/' + '_search',
      searchSTring: JSON.stringify({
        'query': {
          'bool': {
            'must': [
              {
                'multi_match': {
                  'query': query,
                  'auto_generate_synonyms_phrase_query': false,
                  'operator': 'OR',
                  'type': 'phrase_prefix',
                  'max_expansions': 50
                },
              },
              {'term': {'isapproved': true}}
            ],
            'filter': filterData,
            'must_not': []
          }
        },
        'sort': [sortData],
        'size': _pageSize,
        'from': _pageStart
      })
    });
  }

  /**
   * search all items
   *
   * @param _pageSize
   * @param _pageStart
   * @param query
   * @param sort
   * @param filter
   */
  advanceSearch(_pageSize, _pageStart, query, sort = 'createdOn', filter: any = []): any {
    let sortData = {};
    if (sort === 'createdOn') {
      sortData = {'createdon': {'order': 'desc'}};
    } else if (sort === 'title') {
      sortData = {'title.keyword': {'order': 'asc'}};
    } else if (sort === 'rating') {
      sortData = {'rating': {'order': 'desc'}};
    } else {
      sortData = {'createdon': {'order': 'desc'}};
    }
    const filterData = [];
    if (filter.categories && filter.categories.length > 0) {
      filterData.push({
        'terms': {
          'categoryid': filter.categories
        }
      });
    }
    if (filter.subCategories && filter.subCategories.length > 0) {
      filterData.push({
        'terms': {
          'subcategoryid': filter.subCategories
        }
      });
    }
    if (filter.educationalStandard && filter.educationalStandard.length > 0) {
      filterData.push({
        'terms': {
          'educationalstandardid': filter.educationalStandard
        }
      });
    }
    if (filter.educationalUse && filter.educationalUse.length > 0) {
      filterData.push({
        'terms': {
          'educationaluseid': filter.educationalUse
        }
      });
    }
    if (filter.level && filter.level.length > 0) {
      filterData.push({
        'terms': {
          'levelid': filter.level
        }
      });
    }
    if (filter.materialType && filter.materialType.length > 0) {
      filterData.push({
        'terms': {
          'materialtypeid': filter.materialType
        }
      });
    }
    if (filter.copyright && filter.copyright.length > 0) {
      filterData.push({
        'terms': {
          'copyrightid': filter.copyright
        }
      });
    }
    if (filter.educations && filter.educations.length > 0) {
      filterData.push({
        'terms': {
          'educationid': filter.educations
        }
      });
    }
    if (filter.professions && filter.professions.length > 0) {
      filterData.push({
        'terms': {
          'professionid': filter.professions
        }
      });
    }
    return this.http.post(environment.apiUrl + 'Elastic', {
      partialURL: '_search',
      searchSTring: JSON.stringify({
        'query': {
          'bool': {
            'must': [
              {
                'multi_match': {
                  'query': query,
                  'auto_generate_synonyms_phrase_query': false,
                  'operator': 'OR',
                  'type': 'phrase_prefix',
                  'max_expansions': 50
                },
              },
              {'term': {'isapproved': true}}
            ],
            'filter': filterData,
            'must_not': []
          }
        },
        'sort': [sortData],
        'size': _pageSize,
        'from': _pageStart
      })
    });
  }

  /**
   * search item from DB
   *
   * @param query
   * @param type
   * @param pageNumber
   * @param pageSize
   */
  search(query, type, pageNumber, pageSize) {
    if (type === 'Course') {
      return this.http.get(environment.apiUrl + 'Course/SearchCourses/' + query + '/' + pageNumber + '/' + pageSize);
    } else {
      return this.http.get(environment.apiUrl + 'Resource/SearchResources/' + query + '/' + pageNumber + '/' + pageSize);
    }
  }

  /**
   * get rating from db
   *
   * @param data
   */
  getRatings(data) {
    return this.http.post(environment.apiUrl + 'Resource/RatingsByContent', data);
  }
}
