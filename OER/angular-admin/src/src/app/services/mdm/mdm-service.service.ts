import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';

/**
 * Master Data API Service
 */
@Injectable({
  providedIn: 'root'
})
export class MdmServiceService {

  /**
   *
   * @param http contain http details
   */
  constructor(private http: HttpClient) {

  }

  /**
   * get all categories from API
   */
  getAllCategories(value = 0) {
    return this.http.get(environment.apiUrl + 'Categories?IsActive='+value);
  }

  /**
   * get category by id
   * @param id contain id details
   */
  getCategoryById(id) {
    return this.http.get(environment.apiUrl + 'Categories/' + id);
  }

  /**
   * create new category API
   *
   * @param data contain data details
   */
  postCategory(data) {
    const item = {
      name: data.Name,
      name_Ar: data.name_Ar,
      createdBy: data.createdBy,
      weight: data.weight,
      active: (data.active === 'true' || data.active)
    };
    return this.http.post(environment.apiUrl + 'Categories', item);
  }

  /**
   * update existing category API
   *
   * @param id contain id details
   * @param data contain data details
   */
  updateCategory(id, data) {
    const item = {
      name: data.Name,
      name_Ar: data.name_Ar,
      updatedBy: data.createdBy,
      weight: data.weight,
      active: (data.active === 'true' || data.active)
    };
    return this.http.put(environment.apiUrl + 'Categories/' + id, item);
  }

  /**
   * Delete category by API
   *
   * @param id contain id details
   */
  deleteCategory(id) {
    return this.http.delete(environment.apiUrl + 'Categories/' + id);
  }

  /**
   * get all subcategories from API
   */
  getAllSubCategories() {
    return this.http.get(environment.apiUrl + 'SubCategories');
  }

  /**
   * get sub category by API
   *
   * @param id contain id details
   */
  getSubCategoryById(id) {
    return this.http.get(environment.apiUrl + 'SubCategories/' + id);
  }

  /**
   * create new sub category API
   *
   * @param data contain data details
   */
  postSubCategory(data) {
    const item = {
      name: data.Name,
      name_Ar: data.name_Ar,
      createdBy: data.createdBy,
      categoryId: data.categoryId,
      weight: data.weight,
      active: (data.active === 'true' || data.active)
    };
    return this.http.post(environment.apiUrl + 'SubCategories', item);
  }

  /**
   * update existing subcategory API
   * @param id contain id details
   * @param data contain data details
   */
  updateSubCategory(id, data) {
    const item = {
      name: data.Name,
      name_Ar: data.name_Ar,
      updatedBy: data.createdBy,
      categoryId: data.categoryId,
      weight: data.weight,
      active: (data.active === 'true' || data.active)
    };
    return this.http.put(environment.apiUrl + 'SubCategories/' + id, item);
  }

  /**
   * delete sub category by id
   * @param id contain id details
   */
  deleteSubCategory(id) {
    return this.http.delete(environment.apiUrl + 'SubCategories/' + id);
  }

  /**
   * get all eductions
   */
  getAllEducation() {
    return this.http.get(environment.apiUrl + 'Educations');
  }

  /**
   * get education by ID
   *
   * @param id contain id details
   */
  getEducationById(id) {
    return this.http.get(environment.apiUrl + 'Educations/' + id);
  }

  /**
   * create new education
   *
   * @param data contain data details
   */
  postEducation(data) {
    const item = {
      name: data.Name,
      name_Ar: data.name_Ar,
      createdBy: data.createdBy,
      weight: data.weight,
      active: (data.active === 'true' || data.active)
    };
    return this.http.post(environment.apiUrl + 'Educations', item);
  }

  /**
   * update existing education
   *
   * @param id contain id details
   * @param data contain data details
   */
  updateEducation(id, data) {
    const item = {
      name: data.Name,
      name_Ar: data.name_Ar,
      updatedBy: data.createdBy,
      weight: data.weight,
      active: (data.active === 'true' || data.active)
    };
    return this.http.put(environment.apiUrl + 'Educations/' + id, item);
  }

  /**
   * delete education by id
   *
   * @param id contain id details
   */
  deleteEducation(id) {
    return this.http.delete(environment.apiUrl + 'Educations/' + id);
  }

  /**
   * get all grades
   *
   */
  getAllGrades() {
    return this.http.get(environment.apiUrl + 'Institutions');
  }

  /**
   * get grade by id
   *
   * @param id contain id details
   */
  getGradesById(id) {
    return this.http.get(environment.apiUrl + 'Institutions/' + id);
  }

  /**
   * create new grade
   *
   * @param data contain data details
   */
  postGrades(data) {
    const item = {
      name: data.Name,
      name_Ar: data.name_Ar,
      createdBy: data.createdBy,
      active: (data.active === 'true' || data.active)
    };
    return this.http.post(environment.apiUrl + 'Institutions', item);
  }

  /**
   * update existing grade
   *
   * @param id contain id details
   * @param data contain data details
   */
  updateGrades(id, data) {
    const item = {
      name: data.Name,
      name_Ar: data.name_Ar,
      updatedBy: data.createdBy,
      active: (data.active === 'true' || data.active)
    };
    return this.http.put(environment.apiUrl + 'Institutions/' + id, item);
  }

  /**
   * delete grade by id
   *
   * @param id contain id details
   */
  deleteGrades(id) {
    return this.http.delete(environment.apiUrl + 'Institutions/' + id);
  }

  /**
   * get all streams
   *
   */
  getAllStreams() {
    return this.http.get(environment.apiUrl + 'Streams');
  }

  /**
   * get stream by id
   *
   * @param id contain id details
   */
  getStreamsById(id) {
    return this.http.get(environment.apiUrl + 'Streams/' + id);
  }

  /**
   * create new stream
   *
   * @param data contain data details
   */
  postStreams(data) {
    const item = {
      name: data.Name,
      name_Ar: data.name_Ar,
      createdBy: data.createdBy,
      active: (data.active === 'true' || data.active)
    };
    return this.http.post(environment.apiUrl + 'Streams', item);
  }

  /**
   * update existing stream
   *
   * @param id contain id details
   * @param data contain data details
   */
  updateStreams(id, data) {
    const item = {
      name: data.Name,
      name_Ar: data.name_Ar,
      updatedBy: data.createdBy,
      active: (data.active === 'true' || data.active)
    };
    return this.http.put(environment.apiUrl + 'Streams/' + id, item);
  }

  /**
   * delete stream by id
   *
   * @param id contain id details
   */
  deleteStreams(id) {
    return this.http.delete(environment.apiUrl + 'Streams/' + id);
  }

  /**
   * get all language
   *
   */
  getAllLanguages() {
    return this.http.get(environment.apiUrl + 'Professions');
  }

  /**
   * get language by id
   *
   * @param id contain id details
   */
  getLanguagesById(id) {
    return this.http.get(environment.apiUrl + 'Professions/' + id);
  }

  /**
   * create language by id
   *
   * @param data contain data details
   */
  postLanguages(data) {
    const item = {
      name: data.Name,
      name_Ar: data.name_Ar,
      createdBy: data.createdBy,
      weight: data.weight,
      active: (data.active === 'true' || data.active)
    };
    return this.http.post(environment.apiUrl + 'Professions', item);
  }

  /**
   * update existing language by id
   *
   * @param id contain id details
   * @param data contain data details
   */
  updateLanguages(id, data) {
    const item = {
      name: data.Name,
      name_Ar: data.name_Ar,
      updatedBy: data.createdBy,
      weight: data.weight,
      active: (data.active === 'true' || data.active)
    };
    return this.http.put(environment.apiUrl + 'Professions/' + id, item);
  }

  /**
   * delete language by id
   *
   * @param id contain id details
   */
  deleteLanguages(id) {
    return this.http.delete(environment.apiUrl + 'Professions/' + id);
  }

  /**
   * get all copyrights
   *
   */
  getAllCopyrights() {
    return this.http.get(environment.apiUrl + 'Copyrights');
  }

  /**
   * get copyright by id
   *
   * @param id contain id details
   */
  getCopyrightsById(id) {
    return this.http.get(environment.apiUrl + 'Copyrights/' + id);
  }

  /**
   * create new copyright
   *
   * @param data contain data details
   */
  postCopyrights(data) {
    const item = {
      title: data.Name,
      title_Ar: data.name_Ar,
      createdBy: data.createdBy,
      media: data.media,
      protected: data.protected === 'true',
      isResourceProtect: data.isResourceProtect === 'true',
      description: data.description,
      description_Ar: data.description_Ar,
      weight: data.weight,
      active: (data.active === 'true' || data.active)
    };
    return this.http.post(environment.apiUrl + 'Copyrights', item);
  }

  /**
   * update exisiting copyright
   *
   * @param id contain id details
   * @param data contain data details
   */
  updateCopyrights(id, data) {
    const item = {
      title: data.Name,
      title_Ar: data.name_Ar,
      updatedBy: data.createdBy,
      createdBy: data.createdBy,
      media: data.media,
      protected: data.protected === 'true',
      isResourceProtect: data.isResourceProtect === 'true',
      description: data.description,
      description_Ar: data.description_Ar,
      weight: data.weight,
      active: (data.active === 'true' || data.active)
    };
    return this.http.put(environment.apiUrl + 'Copyrights/' + id, item);
  }

  /**
   * delete copyright by id
   *
   * @param id contain id details
   */
  deleteCopyrights(id) {
    return this.http.delete(environment.apiUrl + 'Copyrights/' + id);
  }

  /**
   * get all Materials
   *
   */
  getAllQRC() {
    return this.http.get(environment.apiUrl + 'Materials');
  }

  /**
   * get Materials by id
   *
   * @param id contain id details
   */
  getQRCById(id) {
    return this.http.get(environment.apiUrl + 'Materials/' + id);
  }

  /**
   * create qrc
   *
   * @param Materials contain Materials details
   */
  postQRC(data) {
    return this.http.post(environment.apiUrl + 'Materials', data);
  }

  /**
   * update Materials by id
   *
   * @param id contain id details
   * @param data contain data details
   */
  updateQRC(id, data) {
    return this.http.put(environment.apiUrl + 'Materials/' + id, data);
  }

  /**
   * delete Materials by id
   *
   * @param id contain id details
   */
  deleteQRC(id) {
    return this.http.delete(environment.apiUrl + 'Materials/' + id);
  }

  /**
   * get all  educational standards
   */
  getAllEducationalStandard() {
    return this.http.get(environment.apiUrl + 'EducationalStandard');
  }

  /**
   * get  educational standard by id
   *
   * @param id contain id details
   */
  getEducationalStandardById(id) {
    return this.http.get(environment.apiUrl + 'EducationalStandard/' + id);
  }

  /**
   * create new  educational standard
   *
   * @param data contain data details
   */
  postEducationalStandard(data) {
    const item = {
      standard: data.Name,
      standard_Ar: data.name_Ar,
      updatedBy: data.updatedBy,
      createdBy: data.createdBy,
      weight: data.weight,
      active: (data.active === 'true' || data.active)
    };
    return this.http.post(environment.apiUrl + 'EducationalStandard', item);
  }

  /**
   * update existing  educational standard
   *
   * @param id contain id details
   * @param data contain data details
   */
  updateEducationalStandard(id, data) {
    const item = {
      id: id,
      standard: data.Name,
      standard_Ar: data.name_Ar,
      updatedBy: data.UpdatedBy,
      createdBy: data.createdBy,
      weight: data.weight,
      active: (data.active === 'true' || data.active)
    };
    return this.http.put(environment.apiUrl + 'EducationalStandard', item);
  }

  /**
   * delete educational standard by id
   *
   * @param id contain id details
   */
  deleteEducationalStandard(id) {
    return this.http.delete(environment.apiUrl + 'EducationalStandard?id=' + id);
  }

  /**
   * get all education uses
   */
  getAllEducationalUse() {
    return this.http.get(environment.apiUrl + 'EducationalUse');
  }

  /**
   * get education use by id
   *
   * @param id contain id details
   */
  getEducationalUseById(id) {
    return this.http.get(environment.apiUrl + 'EducationalUse/' + id);
  }

  /**
   * create new education use
   *
   * @param data contain data details
   */
  postEducationalUse(data) {
    const item = {
      name: data.Name,
      name_Ar: data.name_Ar,
      createdBy: data.createdBy,
      weight: data.weight,
      active: (data.active === 'true' || data.active)
    };
    return this.http.post(environment.apiUrl + 'EducationalUse', item);
  }

  /**
   * update existing education use
   *
   * @param id contain id details
   * @param data contain data details
   */
  updateEducationalUse(id, data) {
    const item = {
      id: id,
      name: data.Name,
      name_Ar: data.name_Ar,
      updatedBy: data.UpdatedBy,
      createdBy: data.createdBy,
      weight: data.weight,
      active: (data.active === 'true' || data.active)
    };
    return this.http.put(environment.apiUrl + 'EducationalUse', item);
  }

  /**
   * delete education use by id
   *
   * @param id contain id details
   */
  deleteEducationalUse(id) {
    return this.http.delete(environment.apiUrl + 'EducationalUse?id=' + id);
  }

  /**
   * get all education levels
   */
  getAllEducationLevel() {
    return this.http.get(environment.apiUrl + 'EducationLevel');
  }

  /**
   * get education level by id
   *
   * @param id contain id details
   */
  getEducationLevelById(id) {
    return this.http.get(environment.apiUrl + 'EducationLevel/' + id);
  }

  /**
   * create new education level API
   *
   * @param data contain data details
   */
  postEducationLevel(data) {
    const item = {
      level: data.Name,
      level_Ar: data.name_Ar,
      updatedBy: data.UpdatedBy,
      createdBy: data.createdBy,
      weight: data.weight,
      active: (data.active === 'true' || data.active)
    };
    return this.http.post(environment.apiUrl + 'EducationLevel', item);
  }

  /**
   * Update education level API
   *
   * @param id contain id details
   * @param data contain data details
   */
  updateEducationLevel(id, data) {
    const item = {
      id: id,
      level: data.Name,
      level_Ar: data.name_Ar,
      updatedBy: data.UpdatedBy,
      createdBy: data.createdBy,
      weight: data.weight,
      active: (data.active === 'true' || data.active)
    };
    return this.http.put(environment.apiUrl + 'EducationLevel', item);
  }

  /**
   * delete education level API
   * @param id contain id details
   */
  deleteEducationLevel(id) {
    return this.http.delete(environment.apiUrl + 'EducationLevel?id=' + id);
  }

}
