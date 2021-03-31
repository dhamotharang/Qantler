import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {retry} from 'rxjs/operators';
import {environment} from '../../environments/environment';

/**
 * Resource API Service
 */
@Injectable({
  providedIn: 'root'
})
export class ResourceService {
  /**
   *
   * @param http
   */
  constructor(private http: HttpClient) {
  }

  /**
   *get all categories list
   *
   */
  getCategories(value = 0): Observable<any> {
    return this.http.get(environment.apiUrl + 'Categories?IsActive='+value)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   * get resource master data API
   *
   */
  getResourceMasterData(): Observable<any> {
    return this.http.get(environment.apiUrl + 'Resource/ResourceMasterData')
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   * get education list API
   *
   */
  getEducations(): Observable<any> {
    return this.http.get(environment.apiUrl + 'Educations')
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   *get professions list API
   *
   */
  getProfessions(): Observable<any> {
    return this.http.get(environment.apiUrl + 'Professions')
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   *resource withdrawal API
   *
   * @param id
   */
  contentWithdrawal(id): Observable<any> {
    return this.http.put(environment.apiUrl + 'Course/ContentWithdrawal?contentTypeId=2&contentId=' + id, {});
  }

  /**
   *get resources list API
   *
   * @param pageNumber
   * @param pageSize
   */
  getResources(pageNumber, pageSize): Observable<any> {
    return this.http.get(environment.apiUrl + 'Resource/' + pageNumber + '/' + pageSize)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   *get resource by id
   *
   * @param slug
   */
  getResourceBySlug(slug): Observable<any> {
    return this.http.get(environment.apiUrl + 'Resource/' + slug)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   *get remix reource
   *
   * @param slug
   */
  getRemixVersion(slug): Observable<any> {
    return this.http.get(environment.apiUrl + 'Resource/GetRemixVersion/' + slug)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   *get materials list API
   *
   */
  getMaterials(): Observable<any> {
    return this.http.get(environment.apiUrl + 'Materials')
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   *get sub categories list API
   *
   */
  getSubCategories(): Observable<any> {
    return this.http.get(environment.apiUrl + 'SubCategories')
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   *get Copyrights list API
   *
   */
  getCopyrights(): Observable<any> {
    return this.http.get(environment.apiUrl + 'Copyrights')
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   *check whitelist url API
   *
   * @param url
   * @param id
   */
  checkWhiteListUrl(url, id): Observable<any> {
    return this.http.post(environment.apiUrl + 'URLWhiteListing/ISWhilteListed', {
      url: url,
      requestedBy: id
    })
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   * create whitelist url request
   *
   * @param data
   */
  postWhiteListUrl(data): Observable<any> {
    return this.http.post(environment.apiUrl + 'URLWhiteListing', data);
  }

  /**
   *repost reousrce API
   *
   * @param data
   */
  reportResource(data): Observable<any> {
    return this.http.post(environment.apiUrl + 'Resource/ReportResource', data);
  }

  /**
   *download reousce content API
   *
   * @param data
   */
  downloadContentForResource(data): Observable<any> {
    return this.http.post(environment.apiUrl + 'Resource/DownloadContentForResource', data);
  }

  /**
   *download reousce content API
   *
   * @param contentId
   */
  downloadContent(contentId): Observable<any> {
    return this.http.get(environment.apiUrl + 'ContentMedia/DownloadResourcesFiles/' + contentId + '/2', {
      headers: new HttpHeaders({
        'Content-Type': 'application/zip',
      }), responseType: 'blob'
    });
  }

  /**
   * report resource comment API
   *
   * @param data
   */
  reportResourceComment(data): Observable<any> {
    return this.http.post(environment.apiUrl + 'Resource/ReportResourceComment', data);
  }

  /**
   *comment on resource
   *
   * @param data
   */
  commentResource(data): Observable<any> {
    return this.http.post(environment.apiUrl + 'Resource/CommentOnResource', data);
  }

  /**
   *update resource comment
   *
   * @param data
   */
  updateCommentResource(data): Observable<any> {
    return this.http.post(environment.apiUrl + 'Resource/UpdateResourceComment', data);
  }

  /**
   * delete resource comment
   *
   * @param commentId
   * @param requestedBy
   */
  deleteResourceComment(commentId, requestedBy): Observable<any> {
    return this.http.delete(environment.apiUrl + 'Resource/DeleteResourceComment/' + commentId + '/' + requestedBy);
  }

  /**
   *hide resource comment API
   *
   * @param commentId
   * @param resourceId
   * @param requestedBy
   */
  hideResourceComment(commentId, resourceId, requestedBy): Observable<any> {
    return this.http.get(environment.apiUrl + 'Resource/HideResourceComment/' + commentId + '/' + resourceId + '/' + requestedBy);
  }

  /**
   *rate resource API
   *
   * @param data
   */
  rateResource(data): Observable<any> {
    return this.http.post(environment.apiUrl + 'Resource/RateResource', data);
  }

  /**
   *rate resource alignment API
   *
   * @param data
   */
  rateResourceAlignment(data): Observable<any> {
    return this.http.post(environment.apiUrl + 'Resource/ResourceAlignmentRating', data);
  }

  /**
   *create resource API
   *
   * @param data
   */
  postResource(data): Observable<any> {
    const resource = data;
    let keywords = '';
    if (data.keywords && data.keywords.length > 0) {
      data.keywords.forEach((item, index) => {
        if (index < (data.keywords.length - 1)) {
          keywords += item.value + ', ';
        } else {
          keywords += item.value;
        }
      });
    } else {
      keywords = '';
    }
    const resourceFiles = [];
    if (data.resourceFiles) {
      data.resourceFiles.forEach((item) => {
        resourceFiles.push({
          associatedFile: item.url,
          fileName: item.fileName
        });
      });
    }
    resource.resourceFiles = resourceFiles;
    const postData = {
      title: resource.title,
      categoryId: resource.categoryId != null ? +resource.categoryId : null,
      subCategoryId: resource.subCategoryId != null ? +resource.subCategoryId : null,
      thumbnail: resource.thumbnail,
      readingTime: resource.readingTime != null ? +resource.readingTime : null,
      resourceDescription: resource.resourceDescription,
      keywords: keywords,
      resourceContent: resource.resourceContent,
      materialTypeId: resource.materialTypeId != null ? +resource.materialTypeId : null,
      copyRightId: resource.copyRightId != null ? +resource.copyRightId : null,
      isDraft: resource.isDraft,
      references: resource.references,
      resourceFiles: resource.resourceFiles,
      createdBy: resource.createdBy,
      emailUrl: resource.emailUrl,
      educationalStandardId: resource.educationalStandardId != null ? +resource.educationalStandardId : null,
      educationalUseId: resource.educationalUseId != null ? +resource.educationalUseId : null,
      levelId: resource.levelId != null ? +resource.levelId : null,
      objective: resource.objective,
      resourceSourceId: resource.resourceSourceId != null ? +resource.resourceSourceId : null
    };
    return this.http.post(environment.apiUrl + 'Resource', postData);
  }

  /**
   *update Resource API
   *
   * @param data
   * @param slug
   */
  patchResource(data, slug): Observable<any> {
    const resource = data;
    let keywords = '';
    if (data.keywords && data.keywords.length > 0) {
      data.keywords.forEach((item, index) => {
        if (index < (data.keywords.length - 1)) {
          keywords += item.value + ', ';
        } else {
          keywords += item.value;
        }
      });
    } else {
      keywords = '';
    }
    const resourceFiles = [];
    if (data.resourceFiles) {
      data.resourceFiles.forEach((item) => {
        resourceFiles.push({
          associatedFile: item.url,
          fileName: item.fileName
        });
      });
    }
    resource.resourceFiles = resourceFiles;
    const postData = {
      id: +slug,
      title: resource.title,
      categoryId: resource.categoryId != null ? +resource.categoryId : null,
      subCategoryId: resource.subCategoryId != null ? +resource.subCategoryId : null,
      thumbnail: resource.thumbnail,
      readingTime: resource.readingTime != null ? +resource.readingTime : null,
      resourceDescription: resource.resourceDescription,
      keywords: keywords,
      resourceContent: resource.resourceContent,
      materialTypeId: resource.materialTypeId != null ? +resource.materialTypeId : null,
      copyRightId: resource.copyRightId != null ? +resource.copyRightId : null,
      isDraft: resource.isDraft,
      references: resource.references,
      resourceFiles: resource.resourceFiles,
      createdBy: resource.createdBy,
      emailUrl: resource.emailUrl,
      educationalStandardId: resource.educationalStandardId != null ? +resource.educationalStandardId : null,
      educationalUseId: resource.educationalUseId != null ? +resource.educationalUseId : null,
      levelId: resource.levelId != null ? +resource.levelId : null,
      objective: resource.objective,
      resourceSourceId: resource.resourceSourceId != null ? +resource.resourceSourceId : null
    };
    return this.http.put(environment.apiUrl + 'Resource/' + slug, postData);
  }


  deleteResourceBySlug(slug): Observable<any> {
    return this.http.delete(environment.apiUrl + 'Resource/' + slug)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

}
